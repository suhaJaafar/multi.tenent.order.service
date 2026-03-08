using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using MultiTenantOrderService.Domain.DTOs;
using MultiTenantOrderService.Domain.Enums;
using MultiTenantOrderService.Domain.Identity.Enums;

namespace MultiTenantOrderService.Infrastructure.Utilities;

public static class ExtentionsHelper
{
    public static List<EnumToObject>? EnumValues(this string? value)
    {
        var result = new List<EnumToObject>();
        var getType = Assembly.Load("Rafidain.Branch.Settlement.Models").GetTypes().Where(type => typeof(Enum).IsAssignableFrom(type))
            .FirstOrDefault(x => x?.Name.ToUpper() == value?.Trim().ToUpper());
        if (getType == null) return null;
        foreach (int item in Enum.GetValues(getType))
        {
            var name = Enum.GetName(getType, item);
            result.Add(new EnumToObject { Key = item, Value = getType.GetField(name).GetDescName(name) });
        }

        return result;
    }

    public static string GetDescName(this FieldInfo? field, string name)
    {
        if (field != null)
            if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attr)
                name = attr.Description;

        return name;
    }


    public static string GetEnumDescription(this Enum enumValue)
    {
        var field = enumValue.GetType().GetField(enumValue.ToString());
        if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
            return attribute.Description;

        throw new ArgumentException("Item not found.", nameof(enumValue));
    }

    public static ServiceResponse<bool> SaveFile(this IFormFile file, string path)
    {
        using Stream fileStream = new FileStream(Path.Combine(path, Guid.NewGuid().ToString() + "_" + file.FileName), FileMode.Create);
        file.CopyTo(fileStream);
        return new ServiceResponse<bool>(true);
    }

    public static Guid? GetUserId(this object obj)
    {
        var httpContextAccessor = new HttpContextAccessor();
        var tokenString = httpContextAccessor?.HttpContext?.Request?.Headers["Authorization"].ToString();
        if (string.IsNullOrEmpty(tokenString)) return null;

        var token = new JwtSecurityTokenHandler().ReadJwtToken(httpContextAccessor.HttpContext!.Request
            .Headers["Authorization"].ToString().Replace("Bearer ", ""));
        return Guid.Parse(token.Claims.First(c => c.Type == "id").Value);
    }


    public static UserType? GetUserType(this object obj)
    {
        var httpContextAccessor = new HttpContextAccessor();
        var tokenString = httpContextAccessor?.HttpContext?.Request?.Headers["Authorization"].ToString();
        if (string.IsNullOrEmpty(tokenString)) return null;

        var token = new JwtSecurityTokenHandler()
            .ReadJwtToken(httpContextAccessor.HttpContext!.Request.Headers["Authorization"].ToString()
                .Replace("Bearer ", ""));
        return token.Claims.First(c => c.Type == "feRole").Value switch
        {
            "SuperAdmin" => UserType.SuperAdmin,
            "DistributionCompany" => UserType.DistributionCompany,
            "Station" => UserType.Station,
            "EPaymentCompany" => UserType.EPaymentCompany,
            "Operation" => UserType.Operation,
            "Reconciliation" => UserType.Reconciliation,
            "Authority" => UserType.Authority,
            "Branch" => UserType.Branch,
            _ => null
        };
    }


    public static Guid? GetStationId(this object obj)
    {
        var httpContextAccessor = new HttpContextAccessor();
        var tokenString = httpContextAccessor?.HttpContext?.Request?.Headers["Authorization"].ToString();
        if (string.IsNullOrEmpty(tokenString)) return null;

        var token = new JwtSecurityTokenHandler().ReadJwtToken(httpContextAccessor.HttpContext!.Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty));
        var stationId = token.Claims.FirstOrDefault(c => c.Type == "stationId");
        if (stationId == null) return null;
        return Guid.Parse(stationId.Value);
    }

    public static int? GetGovernorateId(this object obj)
    {
        var httpContextAccessor = new HttpContextAccessor();
        var tokenString = httpContextAccessor?.HttpContext?.Request?.Headers["Authorization"].ToString();
        if (string.IsNullOrEmpty(tokenString)) return null;

        var token = new JwtSecurityTokenHandler().ReadJwtToken(httpContextAccessor.HttpContext!.Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty));
        var governorateId = token.Claims.FirstOrDefault(c => c.Type == "governorateId");
        if (governorateId == null) return null;
        return int.Parse(governorateId.Value);
    }
    
}