namespace MultiTenantOrderService.Domain.Identity.ObjectValues;

public record Email
{
    public string Value { get; init; }
    
    public Email(string value)
    {
        Validate(value);
        Value = value;
    }
    private void Validate(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentNullException("value", "Email is not valid.");
        }

        if (!value.Contains("@"))
        {
            throw new ArgumentException("Email is not valid.");
        }
    }
    public static implicit operator Email(string value)
    {
        return new Email(value);
    }
}