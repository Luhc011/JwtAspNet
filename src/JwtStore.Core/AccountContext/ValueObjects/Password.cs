using JwtStore.Core.SharedContext.ValueObjects;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;

namespace JwtStore.Core.AccountContext.ValueObjects;

public class Password : ValueObject
{
    private const string ValidCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ01234567890";
    private const string SpecialCharacters = "!@#$%^&*(){}[]";
    private const int DefaultSaltSize = 16;
    private const int DefaultKeySize = 32;
    private const int DefaultIterations = 10000;
    private const char SplitChar = '.';
    public string Hash { get; } = string.Empty;
    public string ResetCode { get; } = Guid.NewGuid().ToString("N")[..8].ToUpper();

    protected Password() { }
    public Password(string? text = null)
    {
        if (string.IsNullOrEmpty(text))
            text = Generate();

        Hash = Hashing(text);
    }

    public bool Challenges(string plainTextPassword) => Verify(Hash, plainTextPassword);

    private static string Generate(short lenght = 16, bool includeSpecialChars = true, bool upperCase = false)
    {
        var chars = ValidCharacters;
        if (includeSpecialChars)
            chars += SpecialCharacters;

        var random = new Random();
        var result = new char[lenght];

        for (int i = 0; i < lenght; i++)
            result[i] = chars[random.Next(chars.Length)];

        return upperCase ? new string(result).ToUpper() : new string(result);
    }

    private static string Hashing(string password, short saltSize = DefaultSaltSize, short keySize = DefaultKeySize, int iterations = DefaultIterations, char splitChar = '.')
    {
        if (string.IsNullOrEmpty(password))
            throw new PasswordValidationException("Password should not be null or empty");

        password += Configuration.Secrets.PasswordSaltKey;

        using var algorithm = new Rfc2898DeriveBytes(password, saltSize, iterations, HashAlgorithmName.SHA512);
        var key = Convert.ToBase64String(algorithm.GetBytes(keySize));
        var salt = Convert.ToBase64String(algorithm.Salt);

        return $"{iterations}{splitChar}{salt}{splitChar}{key}";
    }


    private static bool Verify(string hash, string password, short keySize = 32, int iterations = 10000)
    {
        password += Configuration.Secrets.PasswordSaltKey;

        var parts = hash.Split(SplitChar, 3);
        if (parts.Length != 3)
            return false;

        var hashIterations = Convert.ToInt32(parts[0]);
        var salt = Convert.FromBase64String(parts[1]);
        var key = Convert.FromBase64String(parts[2]);

        if (hashIterations != iterations)
            return false;

        using var algorithm = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA512);
        var keyToCheck = algorithm.GetBytes(keySize);

        return keyToCheck.Length == keySize && keyToCheck.AsSpan().SequenceEqual(key);
    }

    public class PasswordValidationException : Exception
    {
        public PasswordValidationException(string message) : base(message) { }
    }
}

