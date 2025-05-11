using Microsoft.AspNetCore.Mvc;
using myProj.Services;

namespace myProj.Middleware;

public class CurrentAuthorMiddleware
{
    private readonly RequestDelegate next;
    public CurrentAuthorMiddleware(RequestDelegate next)
    {
        this.next = next;
    } 
    public async Task Invoke(HttpContext c, [FromHeader] string Authorization)
    {
        string token = Authorization.Split(" ")[1];
        int id = TokenServise.GetAuthorIdByToken(token);
        if(id != -1)
            await next(c);
    }
}