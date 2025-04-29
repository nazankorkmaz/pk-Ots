/*
using System.Net;
using System.Text;
using Ots.Base;

namespace Ots.Api.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;


    public RequestLoggingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
    {
        _next = next;
        _logger = loggerFactory.CreateLogger<RequestLoggingMiddleware>();
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            _logger.LogInformation("Request {method}  -  {url} => {statusCode}   invoked",
                             context.Request?.Method,
                             context.Request?.Path.Value,
                             context.Response?.StatusCode);

        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
        finally
        {
            var originalBodyStream = context.Response.Body;
            var ReqText = await FormatRequest(context.Request);
            var RspText = "";

            using (var responseBody = new MemoryStream())
            {
                try
                {
                    context.Response.Body = responseBody;
                    await _next(context);
                    RspText = await FormatResponse(context?.Response);
                    await responseBody.CopyToAsync(originalBodyStream);

                    // log 
                    _logger.LogInformation($"ResponseCode= {context.Response.StatusCode} ||  Request= {ReqText}  ||  Response= {RspText} ");
                }
                catch (Exception ex)
                {
                    // log 
                    _logger.LogInformation($"ResponseCode= {context.Response.StatusCode} || Error = {ex.ToString()}");
                    await HandleExceptionAsync(context, ex);

                }



            }
        }
    }
    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        await context.Response.WriteAsync(new ErrorDetail()
        {
            StatusCode = context.Response.StatusCode,
            Message = "Internal Server Error"
        }.ToString());
    }

    private async Task<string> FormatRequest(HttpRequest request)
    {
        var bodyAsText = "";
        try
        {
            using (var bodyReader = new StreamReader(request.Body))
            {
                bodyAsText = await bodyReader.ReadToEndAsync();
                request.Body = new MemoryStream(Encoding.UTF8.GetBytes(bodyAsText));
            }
        }
        catch (Exception ex)
        {

        }

        return $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString} {bodyAsText}";
    }
    private async Task<string> FormatResponse(HttpResponse response)
    {
        var sr = new StreamReader(response.Body);
        response.Body.Seek(0, SeekOrigin.Begin);
        var text = await sr.ReadToEndAsync();
        response.Body.Seek(0, SeekOrigin.Begin);

        return $"Response {text}";
    }

}

*/

using Microsoft.IO;
using Serilog;

namespace Ots.Api.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate next;
    private readonly RecyclableMemoryStreamManager recyclableMemoryStreamManager;
    private readonly Action<RequestProfilerModel> requestResponseHandler;
    private const int ReadChunkBufferLength = 4096;


    public RequestLoggingMiddleware(RequestDelegate next,Action<RequestProfilerModel> requestResponseHandler)
    {
        this.next = next;
        this.requestResponseHandler = requestResponseHandler;
        this.recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
    }

    public async Task Invoke(HttpContext context)
    {
        Log.Information("LogRequestLoggingMiddleware.Invoke");

        var model = new RequestProfilerModel
        {
            RequestTime = new DateTimeOffset(),
            Context = context,
            Request = await FormatRequest(context)
        };

        Stream originalBody = context.Response.Body;

        using (MemoryStream newResponseBody = recyclableMemoryStreamManager.GetStream())
        {
            context.Response.Body = newResponseBody;

            await next(context);

            newResponseBody.Seek(0, SeekOrigin.Begin);
            await newResponseBody.CopyToAsync(originalBody);

            newResponseBody.Seek(0, SeekOrigin.Begin);
            model.Response = FormatResponse(context, newResponseBody);
            model.ResponseTime = new DateTimeOffset();
            requestResponseHandler(model);
        }    
    }

    private string FormatResponse(HttpContext context, MemoryStream newResponseBody)
    {
        HttpRequest request = context.Request;
        HttpResponse response = context.Response;

        return $"Http Response Information: {Environment.NewLine}" +
                $"Schema:{request.Scheme} {Environment.NewLine}" +
                $"Host: {request.Host} {Environment.NewLine}" +
                $"Path: {request.Path} {Environment.NewLine}" +
                $"QueryString: {request.QueryString} {Environment.NewLine}" +
                $"StatusCode: {response.StatusCode} {Environment.NewLine}" +
                $"Response Body: {ReadStreamInChunks(newResponseBody)}";
    }

    private async Task<string> FormatRequest(HttpContext context)
    {
        HttpRequest request = context.Request;

        return $"Http Request Information: {Environment.NewLine}" +
                    $"Schema:{request.Scheme} {Environment.NewLine}" +
                    $"Host: {request.Host} {Environment.NewLine}" +
                    $"Path: {request.Path} {Environment.NewLine}" +
                    $"QueryString: {request.QueryString} {Environment.NewLine}" +
                    $"Request Body: {await GetRequestBody(request)}";
    }
    public async Task<string> GetRequestBody(HttpRequest request)
    {
        request.EnableBuffering();
        using (var requestStream = recyclableMemoryStreamManager.GetStream())
        {
            await request.Body.CopyToAsync(requestStream);
            request.Body.Seek(0, SeekOrigin.Begin);
            return ReadStreamInChunks(requestStream);
        }
    }

    private static string ReadStreamInChunks(Stream stream)
    {
        stream.Seek(0, SeekOrigin.Begin);
        string result;
        using (var textWriter = new StringWriter())
        using (var reader = new StreamReader(stream))
        {
            var readChunk = new char[ReadChunkBufferLength];
            int readChunkLength;

            do
            {
                readChunkLength = reader.ReadBlock(readChunk, 0, ReadChunkBufferLength);
                textWriter.Write(readChunk, 0, readChunkLength);
            } while (readChunkLength > 0);

            result = textWriter.ToString();
        }

        return result;
    }


}

public class RequestProfilerModel
{
    public DateTimeOffset RequestTime { get; set; }
    public HttpContext Context { get; set; }
    public string Request { get; set; }
    public string Response { get;  set; }
    public DateTimeOffset ResponseTime { get;  set; }
}