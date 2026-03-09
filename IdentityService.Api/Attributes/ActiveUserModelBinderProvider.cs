using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using IdentityService.Domain.DTOs;
using IdentityService.Domain.Identity.DTOs;

namespace IdentityService.Api.Attributes;
/// <summary>
/// Provider for the ActiveUser model binder
/// </summary>
public class ActiveUserModelBinderProvider : IModelBinderProvider
{
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (context.Metadata.ModelType == typeof(ActiveUserData))
        {
            return new BinderTypeModelBinder(typeof(ActiveUserModelBinder));
        }

        return null;
    }
}

