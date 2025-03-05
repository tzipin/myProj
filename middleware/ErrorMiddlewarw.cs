namespace myProj.Middlewares;

public class Errormiddleware
{
    private readonly RequestDelegate next;

    public ErrorMiddlewarw(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task Invoke(HttpContext c)
    {
        try
        {
            await next(c);
        }
        catch (AplicationExeption ex)
        {
            c.Response.StatusCode = 400;
            await c.Response.WriteAsync(ex.Message);
        }
        catch (Exeption ex)
        {
            c.Response.StatusCode = 500;
            await c.Response.WriteAsync("פנה לתמיכה הטכנית");
        }
    }
}