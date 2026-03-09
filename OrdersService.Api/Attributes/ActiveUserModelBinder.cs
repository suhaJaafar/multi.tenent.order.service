using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OrdersService.Api.DTOs;

namespace OrdersService.Api.Attributes;

public class ActiveUserModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext == null)
        {
            throw new ArgumentNullException(nameof(bindingContext));
        }

        var user = bindingContext.HttpContext.User;

        if (!user.Identity?.IsAuthenticated ?? true)
        {
            bindingContext.Result = ModelBindingResult.Success(null);
            return Task.CompletedTask;
        }

        var activeUser = new ActiveUserData
        {
            UserId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                              ?? user.FindFirst("sub")?.Value 
                              ?? user.FindFirst("id")?.Value 
                              ?? Guid.Empty.ToString()),
            Name = user.FindFirst(ClaimTypes.Name)?.Value 
                ?? user.FindFirst("name")?.Value 
                ?? string.Empty,
            Email = user.FindFirst(ClaimTypes.Email)?.Value 
                 ?? user.FindFirst("email")?.Value 
                 ?? string.Empty
        };

        bindingContext.Result = ModelBindingResult.Success(activeUser);
        return Task.CompletedTask;
    }
}

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
            return new ActiveUserModelBinder();
        }

        return null;
    }
}

