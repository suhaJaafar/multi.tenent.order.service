using System.Globalization;
using MultiTenantOrderService.Domain.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using OpenSearch.Client;

namespace MultiTenantOrderService.Infrastructure.Utilities;
public class OpenSearchHelper
{
    private readonly IConfiguration _configuration;
    private readonly OpenSearchClient _openSearchClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public OpenSearchHelper(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
        var nodeAddress = new Uri(_configuration["AwsOpenSearch:Endpoint"] ?? string.Empty);
        var connectionSettings = new ConnectionSettings(nodeAddress).DefaultIndex($"{configuration["AwsOpenSearch:Index"]}{GetIndexSuffix()}");
        connectionSettings.BasicAuthentication(_configuration["AwsOpenSearch:Username"], _configuration["AwsOpenSearch:Password"]);
        _openSearchClient = new OpenSearchClient(connectionSettings);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public async Task<IndexResponse> Index(LoggerDto loggerDto)
    {
        return await _openSearchClient.IndexAsync(loggerDto, i =>
            i.Index($"{_configuration["AwsOpenSearch:Index"]}_{GetIndexSuffix()}"));
    }

    public LoggerDto GetLoggerModel(HttpContext context = default, string request = default, string response = default, double duration = default, string exception = default, string statusCode = default)
    {
        var log = new LoggerDto
        {
            Request = request ?? string.Empty,
            Response = response ?? string.Empty,
            Duration = duration,
            Exception = exception ?? string.Empty,
            StatusCode = statusCode ?? string.Empty
        };
        if (context != default)
            log.Headers = new IdentityService.Domain.DTOs.Headers
            {
                Host = context.Request.Headers["Host"],
                Accept = context.Request.Headers["Accept"],
                AcceptEncoding = context.Request.Headers["AcceptEncoding"],
                AcceptLanguage = context.Request.Headers["AcceptLanguage"],
                Authorization = context.Request.Headers["Authorization"],
                Connection = context.Request.Headers["Connection"]!,
                ContentLength = context.Request.Headers["ContentLength"]!,
                ContentType = context.Request.Headers["ContentType"]!,
                Origin = context.Request.Headers["Origin"].ToString(),
                Referer = context.Request.Headers["Referer"].ToString(),
                UserAgent = context.Request.Headers["UserAgent"]!,
                RemoteIpAddress = ClientRemoteIpAddress(context),
                RequestUrl = context.Request.Host.Value,
                RequestPath = context.Request.Path,
                Method = context.Request.Method
            };
        
        return log;
    }

    private static string GetIndexSuffix()
    {
        var myCi = new CultureInfo("iq-IQ");
        var myCal = myCi.Calendar;
        var myCwr = myCi.DateTimeFormat.CalendarWeekRule;
        var myFirstDow = myCi.DateTimeFormat.FirstDayOfWeek;
        var weekOfYear = myCal.GetWeekOfYear(DateTime.Now, myCwr, myFirstDow);
        return $"{DateTime.Now.Year}-{DateTime.Now.Month:00}-{weekOfYear}";
    }

    private static string ClientRemoteIpAddress(HttpContext context)
    {
        return context.Request.Headers.TryGetValue("X-Forwarded-For", out var header)
            ? header.ToString()
            : context.Connection.RemoteIpAddress?.MapToIPv4().ToString();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
        }
    }
}