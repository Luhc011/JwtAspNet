using JwtStore.Core.Contexts.AccountContext.UseCases.Authenticate.Contracts;
using JwtStore.Core.Contexts.AccountContext.UseCases.Create;
using JwtStore.Core.Contexts.AccountContext.UseCases.Create.Contracts;
using JwtStore.Infra.Contexts.AccountContext.UseCases.Authenticate;
using JwtStore.Infra.Contexts.AccountContext.UseCases.Create;
using MediatR;

namespace JwtStore.Api.Extensions;

public static class AccountContextExtension
{
    public static void AddAccountContext(this WebApplicationBuilder builder)
    {
        // Create
        builder.Services.AddTransient<IRepository, Repository>();
        builder.Services.AddTransient<IService, Service>();

        // Authenticate
        builder.Services.AddTransient<IAuthenticateRepository, AuthenticateRepository>();
    }
    public static void MapAccountEndPoints(this WebApplication application)
    {
        #region Create
        application.MapPost("api/v1/users", async (Request request, IRequestHandler<Request, Response> handler) =>
        {
            var result = await handler.Handle(request, new CancellationToken());

            return result.IsSuccess
                ? Results.Created($"api/v1/users/{result.Data?.Name}", result.Data)
                : Results.Json(result, statusCode: result.Status);
        });
        #endregion

        #region Authenticate
        application.MapPost("api/v1/authenticate", async (
            Core.Contexts.AccountContext.UseCases.Authenticate.Request request,
            IRequestHandler<
                Core.Contexts.AccountContext.UseCases.Authenticate.Request,
                Core.Contexts.AccountContext.UseCases.Authenticate.Response> handler) =>
        {
            var result = await handler.Handle(request, new CancellationToken());

            if (!result.IsSuccess)
                return Results.Json(result, statusCode: result.Status);

            if (result.Data is null)
                return Results.Json(result, statusCode: 500);

            result.Data.Token = JwtExtension.Generate(result.Data);
            return Results.Ok(result);

        }).RequireAuthorization();
        #endregion

    }

}
