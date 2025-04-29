using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace myProj.Services;

public class TokenServise
{
    private readonly HttpClient httpClient;
    public TokenServise(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }
    private static SymmetricSecurityKey key 
            = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    "SXkSqsKyNUyvGbnHs7ke2NCq8zQzNLW7mPmHbnZZ"));

    private static string issuer = "https://myProg.com";
    public static SecurityToken GetToken(List<Claim> claims) =>
        new JwtSecurityToken(
            issuer,
            issuer,
            claims,
            expires: DateTime.Now.AddDays(30.0),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
    );

    public static TokenValidationParameters 
        GetTokenValidationParameters() =>
        new TokenValidationParameters
        {
            ValidIssuer = issuer,
            ValidAudience = issuer,
            IssuerSigningKey = key,
            ClockSkew = TimeSpan.Zero // remove delay of token when expire
        };

    public static string WriteToken(SecurityToken token) =>
        new JwtSecurityTokenHandler().WriteToken(token);

    public void Token(string token)
    {
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}
 
