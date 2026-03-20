using IdentityService.Application.Abstractions;
using IdentityService.Domain.DTOs;

namespace IdentityService.Infrastructure.Interfaces;

[Obsolete("This interface has been moved to IdentityService.Application.Abstractions. Please use that version instead.")]
public interface IEnumService
{
    ServiceResponse<List<EnumToObject>> GetEnumValues(string enumName);
}

