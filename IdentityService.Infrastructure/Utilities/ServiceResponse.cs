using IdentityService.Application.Abstractions;

namespace IdentityService.Infrastructure.Utilities;

[Obsolete("This class has been moved to IdentityService.Application.Abstractions. Please use that version instead.")]
public class ServiceResponse<T> : Application.Abstractions.ServiceResponse<T>
{
    public ServiceResponse(T value) : base(value) { }
    public ServiceResponse(T value, int count) : base(value, count) { }
    public ServiceResponse(T value, string msg) : base(value, msg) { }
    public ServiceResponse(bool error, string msg) : base(error, msg) { }
}

