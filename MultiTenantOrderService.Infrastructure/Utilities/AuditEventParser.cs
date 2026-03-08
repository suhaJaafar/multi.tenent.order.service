using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace  MultiTenantOrderService.Infrastructure.Utilities
{

    public partial class AuditEventParser
    {
        [JsonProperty("Action")] public Action Action { get; set; }

        [JsonProperty("EventType")] public string EventType { get; set; }

        [JsonProperty("Environment")] public Environment Environment { get; set; }

        [JsonProperty("StartDate")] public DateTimeOffset StartDate { get; set; }

        [JsonProperty("EndDate")] public DateTimeOffset EndDate { get; set; }

        [JsonProperty("Duration")] public string Duration { get; set; }
    }

    public partial class Action
    {
        [JsonProperty("TraceId")] public string TraceId { get; set; }

        [JsonProperty("HttpMethod")] public string HttpMethod { get; set; }

        [JsonProperty("ControllerName")] public string ControllerName { get; set; }

        [JsonProperty("ActionName")] public string ActionName { get; set; }

        [JsonProperty("ActionParameters")] public ActionParameters ActionParameters { get; set; }

        [JsonProperty("RequestUrl")] public Uri RequestUrl { get; set; }

        [JsonProperty("IpAddress")] public string IpAddress { get; set; }

        [JsonProperty("ResponseStatus")] public string ResponseStatus { get; set; }

        [JsonProperty("ResponseStatusCode")] public string ResponseStatusCode { get; set; }

        [JsonProperty("RequestBody")] public RequestBody RequestBody { get; set; }

        [JsonProperty("ResponseBody")] public ResponseBody ResponseBody { get; set; }

        [JsonProperty("Headers")] public Headers Headers { get; set; }

        [JsonProperty("ResponseHeaders")] public ResponseHeaders ResponseHeaders { get; set; }

        [JsonProperty("ModelStateValid")] public bool ModelStateValid { get; set; }

        [JsonProperty("Exception")] public string Exception { get; set; }
    }

    public partial class ActionParameters
    {
        [JsonProperty("form")] public Form Form { get; set; }
    }

    public partial class Form
    {
        [JsonProperty("Username")] public string Username { get; set; }

        [JsonProperty("Password")] public string Password { get; set; }
    }

    public partial class Headers
    {
        [JsonProperty("Accept")] public string Accept { get; set; }

        [JsonProperty("Connection")] public string Connection { get; set; }

        [JsonProperty("Authorization")] public string Authorization { get; set; }

        [JsonProperty("Host")] public string Host { get; set; }

        [JsonProperty("User-Agent")] public string UserAgent { get; set; }

        [JsonProperty("Accept-Encoding")] public string AcceptEncoding { get; set; }

        [JsonProperty("Accept-Language")] public string AcceptLanguage { get; set; }

        [JsonProperty("Content-Type")] public string ContentType { get; set; }

        [JsonProperty("Origin")] public Uri Origin { get; set; }

        [JsonProperty("Referer")] public Uri Referer { get; set; }

        [JsonProperty("Content-Length")] public string ContentLength { get; set; }

        [JsonProperty("sec-ch-ua")] public string SecChUa { get; set; }

        [JsonProperty("sec-ch-ua-mobile")] public string SecChUaMobile { get; set; }

        [JsonProperty("sec-ch-ua-platform")] public string SecChUaPlatform { get; set; }

        [JsonProperty("Sec-Fetch-Site")] public string SecFetchSite { get; set; }

        [JsonProperty("Sec-Fetch-Mode")] public string SecFetchMode { get; set; }

        [JsonProperty("Sec-Fetch-Dest")] public string SecFetchDest { get; set; }
    }

    public partial class RequestBody
    {
        [JsonProperty("Type")] public string Type { get; set; }

        [JsonProperty("Length")] public string Length { get; set; }
    }

    public partial class ResponseBody
    {
        [JsonProperty("Type")] public string Type { get; set; }

        [JsonProperty("Value")] public object Value { get; set; }
    }

    public partial class ResponseHeaders
    {
        [JsonProperty("Content-Type")] public string ContentType { get; set; }
    }

    public partial class Environment
    {
        [JsonProperty("UserName")] public string UserName { get; set; }

        [JsonProperty("MachineName")] public string MachineName { get; set; }

        [JsonProperty("DomainName")] public string DomainName { get; set; }

        [JsonProperty("CallingMethodName")] public string CallingMethodName { get; set; }

        [JsonProperty("AssemblyName")] public string AssemblyName { get; set; }

        [JsonProperty("Culture")] public string Culture { get; set; }
    }
}