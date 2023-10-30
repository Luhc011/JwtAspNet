using Flunt.Notifications;
using Flunt.Validations;

namespace JwtStore.Core.Contexts.AccountContext.UseCases.Authenticate;

public class Specification
{
    public static Contract<Notification> Ensure(Request request)
        => new Contract<Notification>()
            .Requires()
            .IsLowerThan(request.Password.Length, 40, nameof(request.Password), "Password must be at least 40 characters long")
            .IsGreaterThan(request.Password.Length, 6, nameof(request.Password), "Password must be at most 6 characters long")
            .IsEmail(request.Email, nameof(request.Email), "Email is invalid");
}
