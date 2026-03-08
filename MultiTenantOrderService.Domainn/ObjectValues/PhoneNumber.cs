namespace MultiTenantOrderService.Domain.ObjectValues;

public record PhoneNumber
{
    public string Value { get; init; }
    
    public PhoneNumber(string value)
    {
        Validate(value);
        Value = value;
    }
    private void Validate(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentNullException("value", "Phone number is not valid.");
        }

        if (value.Length < 7 || value.Length > 15)
        {
            throw new ArgumentException("Phone number is not valid.");
        }
    }
    public static implicit operator PhoneNumber(string value)
    {
        return new PhoneNumber(value);
    }
    
}