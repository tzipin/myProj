using System.Diagnostics;

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
       Console.WriteLine($"{c.Request.Path}.{c.Request.Method} start");
        var sw = new Stopwatch();
        sw.Start();
        await next(c);
        Console.WriteLine($"{c.Request.Path}.{c.Request.Method} end after {sw.ElapsedMilliseconds}ms");
    }
}

public static partial class Middlewares
{
    public static IApplicationBuilder UseLogMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<LogMiddleware>();
    }
}