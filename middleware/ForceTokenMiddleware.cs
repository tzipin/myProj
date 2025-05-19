using Microsoft.AspNetCore.Mvc;
using myProj.Services;

namespace myProj.Middleware;

public class ForceTokenMiddleware
{
    private readonly RequestDelegate next;
    public ForceTokenMiddleware(RequestDelegate next)
    {
        this.next = next;
    } 
    public async Task Invoke(HttpContext c)
    {
         if (c.Request.Path.StartsWithSegments("/login"))
        {
            await next(c);
            return;
        }
        string authorization = c.Request.Headers["Authorization"];
        string token = authorization.Split(" ")[1];
        int id = TokenServise.GetAuthorIdByToken(token);
        System.Console.WriteLine(id);
        if(id != -1)
            await next(c);
    }
}

public static partial class Middlewares
{
    public static IApplicationBuilder UseForceTokenMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ForceTokenMiddleware>();
    }
}