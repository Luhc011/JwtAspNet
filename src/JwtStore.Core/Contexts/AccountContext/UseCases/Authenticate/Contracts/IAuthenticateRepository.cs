using JwtStore.Core.Contexts.AccountContext.Entities;

namespace JwtStore.Core.Contexts.AccountContext.UseCases.Authenticate.Contracts;

public interface IAuthenticateRepository
{
    Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken);
}
