using JwtStore.Core.Contexts.AccountContext.Entities;
using JwtStore.Core.Contexts.AccountContext.UseCases.Authenticate.Contracts;
using MediatR;

namespace JwtStore.Core.Contexts.AccountContext.UseCases.Authenticate;

public class Handler : IRequestHandler<Request, Response>
{
    private readonly IAuthenticateRepository _repository;

    public Handler(IAuthenticateRepository repository) => _repository = repository;

    public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
    {
        #region 01. Validate request
        try
        {
            var res = Specification.Ensure(request);
            if (!res.IsValid)
                return new Response("Invalid request", 400, res.Notifications);
        }
        catch
        {
            return new Response("Invalid request", 500);
        }
        #endregion

        #region 02. Check if user exists in database
        User? user;

        try
        {
            user = await _repository.GetUserByEmailAsync(request.Email, cancellationToken);
            if (user is null)
                return new Response("Invalid credentials", 400);
        }
        catch
        {
            return new Response("Internal server error", 500);
        }
        #endregion

        #region 03. Check if password is valid
        try
        {
            var valid = user.Password.Challenges(request.Password);
            if (!valid)
                return new Response("Invalid credentials", 400);
        }
        catch
        {
            return new Response("Internal server error", 500);
        }
        #endregion

        #region 04. Check if user is active
        try
        {
            if (!user.Email.Verification.IsActive)
                return new Response("User is not active", 400);
        }
        catch
        {
            return new Response("Internal server error", 500);
        }
        #endregion

        #region 05. Return data
        try
        {
            var data = new ResponseData
            {
                Id = user.Id.ToString(),
                Name = user.Name,
                Email = user.Email,
                Roles = user.Roles.Select(x => x.Name).ToArray()
            };

            return new Response("User authenticated", data);
        }
        catch
        {
            return new Response("We couldn't get the profile data", 500);
        }
        #endregion
    }
}
