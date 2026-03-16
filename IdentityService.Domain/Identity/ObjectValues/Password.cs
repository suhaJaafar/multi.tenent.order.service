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
    }

    public static implicit operator Password(string value)
    {
        return new Password(value);
    }
}