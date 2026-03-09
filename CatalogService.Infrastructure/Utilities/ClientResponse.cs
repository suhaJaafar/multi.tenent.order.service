namespace CatalogService.Infrastructure.Utilities;

public class ClientResponse<T>
{
    public ClientResponse(bool error, string message)
    {
        Error = error;
        Message = message;
    }

    public ClientResponse(T data, string message)
    {
        Data = data;
        Message = message;
    }

    public ClientResponse(T data, int count = 1)
    {
        Error = false;
        Data = data;
        Count = count;
    }

    public bool? Error { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }
    public int? Count { get; set; }
}

