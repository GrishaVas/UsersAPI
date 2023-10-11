using Org.BouncyCastle.Utilities;
using Serilog;
using System.Text;

namespace UsersAPI.Models
{
    public class UsersLogger
    {
        RequestDelegate _next;
        public UsersLogger(RequestDelegate next)
        {
            this._next = next;
            Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
        }
        public async Task InvokeAsync(HttpContext context)
        {
            string requestPath = context.Request.Path.Value;
            string requestBody;
            string requestHeaders = "";
            byte[] bytes;

            foreach (var item in context.Request.Headers)
            {
                requestHeaders += $"    {item.Key} : {item.Value}\n";
            }

            await this._next(context);

            if (context.Request.ContentLength > 0)
            {
                context.Request.EnableBuffering();
                bytes = new byte[context.Request.ContentLength.Value];
                await context.Request.Body.ReadAsync(bytes, 0, bytes.Length);
                requestBody = Encoding.UTF8.GetString(bytes);
                context.Request.Body.Position = 0;
            }
            else requestBody = "null";   
            Log.Information("\nRequestPath:\n   {RequestPath}, \nRequestBody:\n   {RequestBody}, \nRequestHeaders:\n{RequestHeaders}", new object[] { requestPath, requestBody, requestHeaders });
        }
        ~UsersLogger()
        {
            Log.CloseAndFlush();
        }
    }
}
