namespace IdentityService.Infrastructure.Utilities;
public static class CardNumberHelper
{
    public static string Mask(string cardNumber)
    {
        return cardNumber[..6] + "******" + cardNumber[^4..];
    }
}