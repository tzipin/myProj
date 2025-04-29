using Microsoft.AspNetCore.Authentication.JwtBearer;
using myProj.Services;
using myProj.middleware;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
    builder.Services.AddDistributedMemoryCache(); // Add memory cache for session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
    options.Cookie.HttpOnly = true; // Make cookie HTTP only
    options.Cookie.IsEssential = true; // Make the session cookie essential
});
builder.Services.AddControllersWithViews();
    

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(cfg =>
    {
        cfg.RequireHttpsMetadata = false;
        cfg.TokenValidationParameters = 
            TokenServise.GetTokenValidationParameters();
    });

    builder.Services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
    });

    builder.Services.AddAuthorization(cfg => 
    {
        cfg.AddPolicy("Librarian",
            policy => policy.RequireClaim("type", "Librarian"));
        cfg.AddPolicy("Author",
            policy => policy.RequireClaim("type", "Librarian", "Author"));
        cfg.AddPolicy("Level1",
            policy => policy.RequireClaim("Level", "1"));
        cfg.AddPolicy("Level2",
            policy => policy.RequireClaim("Level", "1", "2"));
    });

    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Library", Version = "v1" });
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter JWT with Bearer into field",
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey
        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        { new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer"}
                },
            new string[] {}
        }
        });
    });
    builder.Services.AddServices();
    // builder.Services.AddScoped<AuthorService>(); // Register AuthorService
    // builder.Services.AddScoped<BookService>(); 

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseErrorMiddleware();

//app.UseLogMiddleware();

app.UseDefaultFiles();

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseRouting();

app.UseSession(); 

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
