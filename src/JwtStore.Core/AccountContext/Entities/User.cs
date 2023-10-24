using JwtStore.Core.AccountContext.ValueObjects;
using JwtStore.Core.SharedContext.Entities;

namespace JwtStore.Core.AccountContext.Entities;

public class User : Entity
{
    public string Name { get; private set; } = string.Empty;
    public Email Email { get; private set; } = null!;
    public Password Password { get; private set; } = null!;
    public string Image { get; private set; } = string.Empty;

    protected User() { }
    public User(string name, Email email, Password password)
    {
        Name = name;
        Email = email;
        Password = password;
    }

    public User(string email, string? password = null)
    {
        Email = email;
        Password = new Password(password);
    }

    public void UpdatePassword(string plainTextPassword, string code)
    {
        if (!string.Equals(code.Trim(), Password.ResetCode.Trim(), StringComparison.CurrentCultureIgnoreCase))
            throw new Exception("Invalid reset code.");

        Password = new Password(plainTextPassword);
    }

    public void UpdateEmail(Email email)
    {
        Email = email;
    }

    public void ChangePassword(string oldPassword, string newPassword)
    {
        if (!Password.Challenges(oldPassword))
            throw new Exception("Invalid old password.");

        Password = new Password(newPassword);
    }
}
