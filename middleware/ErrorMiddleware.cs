using System.Net;
using System.Net.Mail;

namespace myProj.middleware;

public class ErrorMiddleware
{
    private readonly RequestDelegate next;

    public ErrorMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task Invoke(HttpContext c)
    {
        try
        {
            await next(c);
        }
        catch (ApplicationException ex)
        {
            c.Response.StatusCode = 400;
            System.Console.WriteLine("aplication error");
            // await SendEmail("tzipi05567@gmail.com", "ERROR", "aplication error");
            // await c.Response.WriteAsync(ex.Message);
        }
        catch (Exception ex)
        {
            c.Response.StatusCode = 500;
            System.Console.WriteLine("server error");
            // await SendEmail("tzipi05567@gmail.com", "ERROR", "server error");
            // await c.Response.WriteAsync("פנה לתמיכה הטכנית");
        }
    }

    public async Task SendEmail(string toEmail, string subject, string body)
    {
        var fromEmail = "tzipi05567@gmail.com";
        var fromPassword = "mhph2005";

        var smtpClient = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            Credentials = new NetworkCredential(fromEmail, fromPassword),
            EnableSsl = true,
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(fromEmail),
            Subject = subject,
            Body = body,
            IsBodyHtml = true,
        };
        mailMessage.To.Add(toEmail);

        await smtpClient.SendMailAsync(mailMessage);
    }
}


public static partial class Middlewares
{
    public static IApplicationBuilder UseErrorMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ErrorMiddleware>();
    }
}

