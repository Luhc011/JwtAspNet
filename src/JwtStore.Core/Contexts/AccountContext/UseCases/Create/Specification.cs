﻿using Flunt.Notifications;
using Flunt.Validations;

namespace JwtStore.Core.Contexts.AccountContext.UseCases.Create;

public static class Specification
{
    public static Contract<Notification> Ensure(Request request)
        => new Contract<Notification>()
            .Requires()
            .IsLowerThan(request.Name.Length, 40, nameof(request.Name), "Name must be at least 40 characters long")
            .IsGreaterThan(request.Name.Length, 3, nameof(request.Name), "Name must be at most 3 characters long")
            .IsEmail(request.Email, nameof(request.Email), "Email is invalid")
            .IsLowerThan(request.Password.Length, 40, nameof(request.Password), "Password must be at least 40 characters long")
            .IsGreaterThan(request.Password.Length, 6, nameof(request.Password), "Password must be at most 6 characters long");
}
