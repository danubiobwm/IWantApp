using IWantApp.Endpoints.Categories;
using IWantApp.Infra.Data;
using Microsoft.AspNetCore.Identity;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddSqlServer<ApplicationDbContext>(
            builder.Configuration["ConnectionStrings:IWantDb"]);
        builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
        {
            options.Password.RequireNonAlphanumeric = true;
           options.Password.RequireDigit = true;
            options.Password.RequireUppercase = true;
         })
            .AddEntityFrameworkStores<ApplicationDbContext>();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.MapMethods(EmployeesPost.Template, EmployeesPost.Methods, EmployeesPost.Handle);
        app.MapMethods(CategoryGetAll.Template, CategoryGetAll.Methods, CategoryGetAll.Handle);
        app.MapMethods(CategoryPut.Template, CategoryPut.Methods, CategoryPut.Handle);
        app.MapMethods(EmployeePost.Template, EmployeePost.Methods, EmployeePost.Handle);

        app.Run();
    }
}