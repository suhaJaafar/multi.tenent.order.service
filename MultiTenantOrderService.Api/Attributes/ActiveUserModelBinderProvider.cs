using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using MultiTenantOrderService.Domain.DTOs;
using MultiTenantOrderService.Domain.Identity.DTOs;

namespace MultiTenantOrderService.Api.Attributes;
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

