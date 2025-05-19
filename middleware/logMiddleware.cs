using System.Diagnostics;
using myProj.Services;
using Serilog;

namespace myProj.middleware;

public class LogMiddleware
{
    private readonly RequestDelegate next;

    public LogMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task Invoke(HttpContext c)
    {
         var logFolder = $"logs/{DateTime.Now:yyyy-MM}";
        if (!Directory.Exists(logFolder))
        {
            Directory.CreateDirectory(logFolder);
        }

        Log.Logger = new LoggerConfiguration()
            .WriteTo.File($"{logFolder}/log.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();
System.Console.WriteLine($"method: {c.Request.Method}");
        Log.Information($"method: {c.Request.Method}");
        Log.Information($"controller: {c.Request.Path}");
        Log.Information($"start time: {DateTime.Now}");
        if (CurrentAuthor.GetCurrentAuthor() != null)
        {
            Log.Information($" client: {CurrentAuthor.GetCurrentAuthor().Name}");
        }
        else
        {
            Log.Information("client: Anonymous");
        }
        var sw = new Stopwatch();
        sw.Start();
       try
       {
            await next(c);
       }
       catch (Exception ex)
       {
            Log.Error($"error: {ex.Message}");
       }
       finally
       {
            sw.Stop();
            Log.Information($"end time: {DateTime.Now}");
            Log.Information($"end after {sw.ElapsedMilliseconds}ms");
            Log.Information($"status code: {c.Response.StatusCode}");
            Log.Information("--------------------------------------------------");
       }

       
        Log.CloseAndFlush();
    }
}

public static partial class Middlewares
{
    public static IApplicationBuilder UseLogMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<LogMiddleware>();
    }
}