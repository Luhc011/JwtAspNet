using JwtStore.Core.SharedContext.Extensions;
using JwtStore.Core.SharedContext.ValueObjects;
using System.Text.RegularExpressions;

namespace JwtStore.Core.AccountContext.ValueObjects;

public partial class Email : ValueObject
{
    private const string Pattern = @"^[\w.-]+@[\w-]+\.[a-z]{2,3}$";

    #region Constructors
    public Email(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
            throw new ArgumentNullException(nameof(address), "Invalid email address");

        Address = address.Trim().ToLowerInvariant();

        if (Address.Length < 5)
            throw new ArgumentException("Invalid email address", nameof(address));

        if (!EmailRegex().IsMatch(Address))
            throw new ArgumentException("Invalid email address", nameof(address));

        Address = address;
    }
    #endregion

    #region Properties
    public string Address { get; }
    public string Hash => Address.ToBase64();
    public Verification Verification { get; private set; } = new();
    #endregion

    #region Operators
    public static implicit operator string(Email email) => email.ToString();
    public static implicit operator Email(string address) => new(address);
    public void ResendVerification() => Verification = new();
    #endregion

    public override string ToString() => Address.Trim().ToLowerInvariant();

    #region Regex
    [GeneratedRegex(Pattern)]
    private static partial Regex EmailRegex();
    #endregion
}
