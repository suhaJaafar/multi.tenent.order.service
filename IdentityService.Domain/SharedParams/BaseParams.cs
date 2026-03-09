namespace IdentityService.Domain.FiltersParams;
public class BaseParams
{
    public Guid? Id { get; set; }
    public int Skip { get; set; } = 0;
    public int Take { get; set; } = 50;

    private string? _search;
    public string? SearchTerm
    {
        get => _search;
        set => _search = string.IsNullOrEmpty(value) ? value : value.ToLower();
    }
}