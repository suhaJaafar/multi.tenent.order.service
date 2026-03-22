namespace IdentityService.Application.Abstractions;

public interface IGenericHttpClient
{
    Task<ServiceResponse<TResult>> PostRequest<TRequestBody, TResult>(string endPoint, TRequestBody body, CancellationToken cancellationToken,
        int requestNumber)
        where TResult : class where TRequestBody : class;
    Task<ServiceResponse<TResult>> GetRequest<TResult>(string settings, string endPoint, CancellationToken cancellationToken) where TResult : class;

    Task<byte[]> GetRequestFile<TResult>(string settings, string endPoint, CancellationToken cancellationToken);
}

