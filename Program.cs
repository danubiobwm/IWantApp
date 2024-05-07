using IWantApp.Endpoints.Categories;
using IWantApp.Endpoints.Security;
using IWantApp.Infra.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Configuration
        builder.Configuration.AddJsonFile("appsettings.json", optional: false);

        // Database
        builder.Services.AddSqlServer<ApplicationDbContext>(
            builder.Configuration["ConnectionStrings:IWantDb"]);

        // Identity
        builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
    {
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireDigit = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
        options.Password.RequiredLength = 3;
    }).AddEntityFrameworkStores<ApplicationDbContext>();


        // Authorization
        builder.Services.AddAuthorization(options =>
        {
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
              .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
              .RequireAuthenticatedUser()
              .Build();
            options.AddPolicy("EmployeePolicy", p =>
                p.RequireAuthenticatedUser().RequireClaim("EmployeeCode"));
        });


        // Authentication
        builder.Services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateActor = true,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero,
                ValidIssuer = builder.Configuration["JwtBearerTokenSettings:Issuer"],
                ValidAudience = builder.Configuration["JwtBearerTokenSettings:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtBearerTokenSettings:SecretKey"]))
            };
            });


        // Add other dependencies
        builder.Services.AddScoped<QueryAllUsersWithClaimName>();


        // Swagger
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();


        var app = builder.Build();


        // Swagger in development
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // Middleware
        app.UseAuthentication();
        app.UseAuthorization();


        app.UseHttpsRedirection();
        // Endpoint mappings
        app.MapMethods(CategoryPost.Template, CategoryPost.Methods, CategoryPost.Handle);
        app.MapMethods(CategoryGetAll.Template, CategoryGetAll.Methods, CategoryGetAll.Handle);
        app.MapMethods(CategoryPut.Template, CategoryPut.Methods, CategoryPut.Handle);
        app.MapMethods(EmployeesPost.Template, EmployeesPost.Methods, EmployeesPost.Handle);
        app.MapMethods(EmployeeGetAll.Template, EmployeeGetAll.Methods, EmployeeGetAll.Handle);
        app.MapMethods(TokenPost.Template, TokenPost.Methods, TokenPost.Handle);



        app.Run();
    }
}
