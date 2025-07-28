using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LSE.TradeApi.Middlewares
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _logFilePath = "Logs/request_response_log.txt";

        public RequestResponseLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
            Directory.CreateDirectory(Path.GetDirectoryName(_logFilePath));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var request = context.Request;
            var stopwatch = Stopwatch.StartNew();

            // Read request body
            string requestBody = "";
            if (request.ContentLength > 0 && request.Body.CanSeek)
            {
                request.EnableBuffering();
                request.Body.Position = 0;
                using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
                requestBody = await reader.ReadToEndAsync();
                request.Body.Position = 0;
            }

            // Read response
            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            await _next(context);
            stopwatch.Stop();

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            var logText = new StringBuilder();
            logText.AppendLine("=== HTTP LOG ENTRY ===");
            logText.AppendLine($"Timestamp     : {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}");
            logText.AppendLine($"Request Path  : {request.Method} {request.Path}{request.QueryString}");
            logText.AppendLine($"Request Body  : {requestBody}");
            logText.AppendLine($"Response Code : {context.Response.StatusCode}");
            logText.AppendLine($"Response Body : {responseText}");
            logText.AppendLine($"Elapsed Time  : {stopwatch.ElapsedMilliseconds} ms");
            logText.AppendLine("=======================");
            logText.AppendLine();

            await File.AppendAllTextAsync(_logFilePath, logText.ToString());

            await responseBody.CopyToAsync(originalBodyStream);
        }
    }
}
