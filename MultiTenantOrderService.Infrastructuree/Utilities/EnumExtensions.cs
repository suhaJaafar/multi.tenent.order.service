

namespace MultiTenantOrderService.Infrastructure.Utilities;
public static class EnumExtensions
{
    // public static string EnumToStringValue(this Enum value)
    // {
    //     var enumType = value.GetType();
    //     if (EnumValueMap.ContainsKey(enumType) && EnumValueMap[enumType].ContainsKey(value))
    //     {
    //         return EnumValueMap[enumType][value];
    //     }
    //
    //     return value.ToString();
    // }


    // build function Convert the string representations to enum values 
    public static List<TEnum> ConvertStringToEnum<TEnum>(string enumString) where TEnum : struct, Enum
    {
        return enumString
            .Split(',')
            .Select(s => Enum.TryParse<TEnum>(s, out var status) ? status : (TEnum?)null)
            .Where(s => s != null)
            .Select(s => s.Value)
            .ToList();
    }


    // public static List<SettlementStatus> ConvertToSettlementStatusList(string commaSeparatedEnumValues)
    // {
    //     return commaSeparatedEnumValues
    //         .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
    //         .Select(s => Enum.TryParse<SettlementStatus>(s.Trim(), true, out var status) ? status : (SettlementStatus?)null)
    //         .Where(s => s != null)
    //         .Select(s => s.Value)
    //         .ToList();
    // }


    // private static readonly Dictionary<Type, Dictionary<Enum, string>> EnumValueMap = new Dictionary<Type, Dictionary<Enum, string>>
    // {
    //     {
    //         typeof(MsgType), new Dictionary<Enum, string>
    //         {
    //             { MsgType.Ms0110, "0110" },
    //             { MsgType.Ms0100, "0100" },
    //             { MsgType.Ms0410, "0410" },
    //             { MsgType.Ms0230, "0230" },
    //             { MsgType.Ms2048, "2048" }
    //         }
    //     },
    //     {
    //         typeof(EdcFlag), new Dictionary<Enum, string>
    //         {
    //             { EdcFlag.D, "D" },
    //             { EdcFlag.N, "N" },
    //             { EdcFlag.Y, "Y" },
    //             { EdcFlag.H, "H" },
    //             { EdcFlag.R, "R" },
    //             { EdcFlag.A, "A" },
    //             { EdcFlag.B, "B" }
    //         }
    //     },
    // };
}