using JwtStore.Core.Contexts.AccountContext.Entities;
using JwtStore.Core.Contexts.AccountContext.UseCases.Authenticate.Contracts;
using JwtStore.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace JwtStore.Infra.Contexts.AccountContext.UseCases.Authenticate;

public class AuthenticateRepository : IAuthenticateRepository
{
    private readonly AppDbContext _context;

    public AuthenticateRepository(AppDbContext context) => _context = context;

    public async Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
        => await _context
            .Users
            .AsNoTracking()
            .Include(x => x.Roles)
            .FirstOrDefaultAsync(x => x.Email.Address == email, cancellationToken);
}
