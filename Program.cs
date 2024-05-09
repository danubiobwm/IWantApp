using IWantApp.Endpoints.Categories;
using IWantApp.Endpoints.Security;
using IWantApp.Infra.Data;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using IWantApp.Endpoints.Products;
using System.Text.Json;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        //log para db
        builder.Host.UseSerilog((context, configuration) =>
        {
            configuration
                .WriteTo.Console()
                .WriteTo.MSSqlServer(
                    context.Configuration["ConnectionStrings:IWantDb"],
                    sinkOptions: new MSSqlServerSinkOptions()
                    {
                        AutoCreateSqlTable = true,
                        TableName = "LogAPI"
                    });
        });

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
        app.MapMethods(ProductPost.Template, ProductPost.Methods, ProductPost.Handle);
        app.MapMethods(ProductGetAll.Template, ProductGetAll.Methods, ProductGetAll.Handle);


        //Filter Error
        app.UseExceptionHandler("/error");
        app.Map("/error", (HttpContext http) => {

            var error = http.Features?.Get<IExceptionHandlerFeature>()?.Error;

            if (error != null)
            {
                if (error is SqlException)
                    return Results.Problem(title: "Database out", statusCode: 500);
                else if (error is BadHttpRequestException)
                    return Results.Problem(title: "Error to convert data to other type. See all the information sent", statusCode: 500);
            }

            return Results.Problem(title: "An error ocurred", statusCode: 500);
        });

        app.Run();
    }
}
