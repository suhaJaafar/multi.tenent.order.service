namespace IdentityService.Domain.DTOs;
public class LoggerDto
{
    public string Request { get; set; }
    public string Response { get; set; }
    public Headers Headers { get; set; }
    public string StatusCode { get; set; }
    public double Duration { get; set; }
    public string Exception { get; set; }
    public DateTime CreateDate { get; set; } = DateTime.Now;
}

public class Headers
{
    public string Host { get; set; }
    public string Accept { get; set; }
    public string AcceptEncoding { get; set; }
    public string AcceptLanguage { get; set; }
    public string Authorization { get; set; }
    public string Connection { get; set; }
    public string ContentLength { get; set; }
    public string ContentType { get; set; }
    public string Origin { get; set; }
    public string Referer { get; set; }
    public string UserAgent { get; set; }
    public string RemoteIpAddress { get; set; }
    public string RequestUrl { get; set; }
    public string RequestPath { get; set; }
    public string Method { get; set; }
}
