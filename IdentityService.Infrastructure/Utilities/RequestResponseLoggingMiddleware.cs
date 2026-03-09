using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net;
using IdentityService.Domain.DTOs;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace IdentityService.Infrastructure.Utilities;

public class RequestResponseLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly OpenSearchHelper _openSearchHelper;

    public RequestResponseLoggingMiddleware(RequestDelegate next, OpenSearchHelper openSearchHelper)
    {
        _next = next;
        _openSearchHelper = openSearchHelper;
    }

    public async Task Invoke(HttpContext context)
    {
        var startRequestDate = DateTime.Now;
        var requestContent = await LogRequest(context);
        await LogResponse(context, requestContent, startRequestDate);
    }

    private static async Task<string> LogRequest(HttpContext context)
    {
        try
        {
            context.Request.HttpContext.Request.EnableBuffering();
            if (context.Request.ContentType != null && context.Request.ContentType.Trim().ToLower().Contains("json"))
                return await new StreamReader(context.Request.HttpContext.Request.Body)
                    .ReadToEndAsync();
            else if (!string.IsNullOrWhiteSpace(context.Request.QueryString.Value))
                return context.Request.QueryString.Value;
            else if (context.Request.ContentType != null &&
                     context.Request.ContentType.Trim().ToLower().Contains("multipart"))
                return "File Content - " + context.Request.ContentType;
            else return "Empty Content";
        }
        finally
        {
            // return to position 0
            context.Request.HttpContext.Request.Body.Position = 0;
        }
    }

    private async Task LogResponse(HttpContext context, string requestContent, DateTime requestDate)
    {
        var originalBody = context.Response.Body;
        await using var newBody = new MemoryStream();
        context.Response.Body = newBody;
        var loggerDto = _openSearchHelper.GetLoggerModel(context: context, request: requestContent);
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, loggerDto, requestDate, ex);
        }
        finally
        {
            var duration = DateTime.Now - requestDate;
            newBody.Seek(0, SeekOrigin.Begin);
            var responseContent = "Empty Content";
            if (context.Response.ContentType != null && context.Response.ContentType.Trim().ToLower().Contains("json"))
                responseContent = await new StreamReader(context.Response.Body).ReadToEndAsync();
            loggerDto.Response = responseContent;
            loggerDto.Duration = duration.TotalMilliseconds;
            loggerDto.StatusCode = context.Response.StatusCode.ToString();
            await _openSearchHelper.Index(loggerDto);
            newBody.Seek(0, SeekOrigin.Begin);
            await newBody.CopyToAsync(originalBody);
        }
    }
    private static Task HandleExceptionAsync(HttpContext context, LoggerDto loggerDto, DateTime requestDate, Exception ex)
    {
        var env = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        var duration = DateTime.Now - requestDate;
        var result = JsonConvert.SerializeObject(new 
        {
            Error = true,
            Message = env == Environments.Production ? "Something Went Wrong!," + ex.Message : $"{ex.Message} {ex.StackTrace}"
        });
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        loggerDto.Exception = ex.StackTrace ?? string.Empty;
        loggerDto.Duration = duration.TotalMilliseconds;
        return context.Response.WriteAsync(result);
    }
}
public static class RequestResponseLoggingMiddlewareExtensions
{
    public static void UseRequestResponseLogging(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<RequestResponseLoggingMiddleware>();
    }
}