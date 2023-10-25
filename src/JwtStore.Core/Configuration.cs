namespace JwtStore.Core;

public static class Configuration
{
    public static SecretsConfiguration Secrets { get; set; } = new();
    public static DbConfiguration Database { get; set; } = new();
    public static EmailConfiguration Email { get; set; } = new();
    public static SendGridConfiguration SendGrid { get; set; } = new();

    public class DbConfiguration
    {
        public string ConnectionString { get; set; } = string.Empty;
        //public string DatabaseName { get; set; } = string.Empty;
        //public string CollectionName { get; set; } = string.Empty;
    }

    public class SecretsConfiguration
    {
        public string ApiKey { get; set; } = string.Empty;
        public string JwtPrivateKey { get; set; } = string.Empty;
        public string PasswordSaltKey { get; set; } = string.Empty;
    }

    public class EmailConfiguration
    {
        public string DefaultFromEmail { get; set; } = "lucas@teste.io";
        public string FromName { get; set; } = "Lucas.io";
    }

    public class SendGridConfiguration
    {
        public string ApiKey { get; set; } = string.Empty;
    }
}
