using IdentityService.Domain.DTOs;

namespace IdentityService.Application.Abstractions;

public interface IEnumService
{
    ServiceResponse<List<EnumToObject>> GetEnumValues(string enumName);
}

