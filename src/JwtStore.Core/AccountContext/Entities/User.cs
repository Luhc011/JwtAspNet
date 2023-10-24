using JwtStore.Core.AccountContext.ValueObjects;
using JwtStore.Core.SharedContext.Entities;

namespace JwtStore.Core.AccountContext.Entities;

public class User : Entity
{
    public User(Email email)
    {
        Email = email;
    }
    public string Name { get; private set; } = string.Empty;
    public Email Email { get; }
}
