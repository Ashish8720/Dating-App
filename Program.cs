using Dating_App.Data;
using Dating_App.Interfaces;
using Dating_App.Middleware;
using Dating_App.Repository;
using Dating_App.Services;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

//get the connection string from app settings
var conStrBuilder = new SqlConnectionStringBuilder
(builder.Configuration.GetConnectionString("DatingDBConnection"));

//set up connection string with password from secret manager
conStrBuilder.Password = builder.Configuration["DBPassword"];

//build the connection string with password 
var connStr = conStrBuilder.ConnectionString;

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(connStr);
});

// Read JWT secret (ensure you have "JWT_Secret" in appsettings or user-secrets)
var jwtKey = builder.Configuration["JWT_Secret"] 
             ?? throw new InvalidOperationException("JWT_Secret configuration missing");

// Register authentication and set the default schemes to JwtBearer
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = true;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
});

//register the jwt token service
builder.Services.AddScoped<ITokenService, TokenService>();

//register the account service
builder.Services.AddScoped<IAccountService, AccountService>();

//register the member repository
builder.Services.AddScoped<IMemberRepository, MemberRepository>();

//configure CORS to allow requests from Angular app
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .WithOrigins("http://localhost:4200");
    });
}); 

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Dating App API", Version = "v1" });

    // Define the Bearer auth scheme that's displayed in Swagger UI
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    // Require Bearer token for all operations (will show the lock icon / Authorize button)
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" },
                Scheme = "bearer",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Dating App API v1");
        // optional: c.RoutePrefix = "swagger"; // if you want /swagger
    });
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "Data/Assets")),
    RequestPath = "/assets"
});
//add custom exception middleware
app.UseMiddleware<ExceptionMiddleware>();

//enable CORS
app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

// Ensure authentication middleware runs before authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

//apply migrations at startup
using var scope = app.Services.CreateScope();   
var services = scope.ServiceProvider;
try
{
    //apply pending migrations
    var context = services.GetRequiredService<AppDbContext>();
    await context.Database.MigrateAsync();
    await SeedData.SeedUsers(context);
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred during migration");
}

app.Run();
