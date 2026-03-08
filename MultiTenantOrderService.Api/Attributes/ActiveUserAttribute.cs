using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MultiTenantOrderService.Api.Attributes
{
    
}
/// <summary>
/// Attribute to automatically bind the current authenticated user's data from JWT claims.
/// This parameter is hidden from Swagger/API documentation as it's populated automatically.
/// </summary>
/// <example>
/// Usage: public async Task&lt;IActionResult&gt; MyAction([ActiveUser] ActiveUserData activeUser)
/// </example>
[AttributeUsage(AttributeTargets.Parameter)]
public class ActiveUserAttribute : Attribute, IModelNameProvider, IBindingSourceMetadata
{
    public BindingSource BindingSource => BindingSource.Custom;
    public string? Name => null;
}

