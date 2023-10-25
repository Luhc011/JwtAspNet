using JwtStore.Core;
using JwtStore.Core.Contexts.AccountContext.Entities;
using JwtStore.Core.Contexts.AccountContext.UseCases.Create.Contracts;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace JwtStore.Infra.Contexts.AccountContext.UseCases.Create;

public class Service : IService
{
    public async Task SendConfirmationEmailAsync(User user, CancellationToken token)
    {
        var client = new SendGridClient(Configuration.SendGrid.ApiKey);
        var from = new EmailAddress(Configuration.Email.DefaultFromEmail, Configuration.Email.FromName);
        var subject = "Confirm your email" + user.Email;
        var to = new EmailAddress(user.Email, user.Name);
        var plainTextContent = $"Hi {user.Name}, please confirm your email by clicking on this link: {user.Email.Verification.Code}";
        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, plainTextContent);
        await client.SendEmailAsync(msg, token);
    }
}
