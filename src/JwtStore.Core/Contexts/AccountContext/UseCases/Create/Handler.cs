using static JwtStore.Core.Contexts.AccountContext.UseCases.Create.Response;
using JwtStore.Core.Contexts.AccountContext.UseCases.Create.Contracts;
using JwtStore.Core.Contexts.AccountContext.ValueObjects;
using JwtStore.Core.Contexts.AccountContext.Entities;
using MediatR;

namespace JwtStore.Core.Contexts.AccountContext.UseCases.Create;

public class Handler : IRequestHandler<Request, Response>
{
    private readonly IRepository _repository;
    private readonly IService _service;

    public Handler(IRepository repository, IService service)
    {
        _repository = repository;
        _service = service;
    }

    public async Task<Response> Handle(Request request, CancellationToken token)
    {
        #region 01. Validate request
        try
        {
            var response = Specification.Ensure(request);
            if (!response.IsValid)
                return new Response("Invalid request", 400, response.Notifications);
        }
        catch
        {
            return new Response("Internal server error", 500);
        }
        #endregion

        #region 02. Generate objects
        Email email;
        Password password;
        User user;

        try
        {
            email = new Email(request.Email);
            password = new Password(request.Password);
            user = new User(request.Name, email, password);
        }
        catch (Exception ex)
        {
            return new Response(ex.Message, 400);
        }
        #endregion

        #region 03. Check if user exists in database
        try
        {
            var exists = await _repository.AnyAsync(request.Email, token);
            if (exists)
                return new Response("Email already in use", 400);
        }
        catch
        {
            return new Response("Internal server error", 500);
        }

        #endregion

        #region 04. Persist user
        try
        {
            await _repository.SaveAsync(user, token);
        }
        catch
        {
            return new Response("Internal server error", 500);
        }
        #endregion

        #region 05. Send confirmation email
        try
        {
            await _service.SendConfirmationEmailAsync(user, token);
        }
        catch
        {
            return new Response("Internal server error", 500);
        }
        #endregion

        return new Response("User created successfully", new ResponseData(user.Id, user.Name, user.Email));
    }
}
