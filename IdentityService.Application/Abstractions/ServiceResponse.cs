namespace IdentityService.Application.Abstractions;

public class ServiceResponse<T>
{
    public ServiceResponse(T value)
    {
        Error = false;
        Value = value;
        Count = 1;
    }

    public ServiceResponse(T value, int count)
    {
        Value = value;
        Count = count;
    }

    public ServiceResponse(T value, string msg)
    {
        Value = value;
        ResponseMessage = msg;
    }

    public ServiceResponse(bool error, string msg)
    {
        Error = error;
        ResponseMessage = msg;
    }

    public T Value { get; set; }
    public bool Error { get; set; }
    public string ResponseMessage { get; set; }
    public int Count { get; set; }
}

