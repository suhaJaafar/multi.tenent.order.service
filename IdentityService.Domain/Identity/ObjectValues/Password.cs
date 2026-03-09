namespace IdentityService.Domain.Identity.ObjectValues;

public record Password
{
    public string Value { get; init; }

    public Password(string value)
    {
        Validate(value);
        Value = value;
    }

    private void Validate(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentNullException("value", "Password is not valid.");
        }

        if (value.Length < 8)
        {
            throw new ArgumentException("Password must be at least 8 characters long.");
        }
    }

    public static implicit operator Password(string value)
    {
        return new Password(value);
    }
}