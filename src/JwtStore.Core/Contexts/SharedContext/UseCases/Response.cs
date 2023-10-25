using Flunt.Notifications;

namespace JwtStore.Core.Contexts.SharedContext.UseCases;

public abstract class ResponseBase
{
    public string Message { get; set; } = string.Empty;
    public int Status { get; set; } = 400;
    public bool IsSuccess => Status >= 200 && Status <= 299;
    public IEnumerable<Notification>? Notifications { get; set; }
}
