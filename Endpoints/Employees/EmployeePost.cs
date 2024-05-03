
using IWantApp.Endpoints;
using IWantApp.Endpoints.Employees;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;


public class EmployeePost
{
    public static string Template => "/employees";

    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };

    public static Delegate Handle => Action;

    public static IResult Action(EmployeResquest employeResquest, UserManager<IdentityUser> userManager)
    {

       var user = new IdentityUser { UserName = employeResquest.Email, Email = employeResquest.Email };

       var result = userManager.CreateAsync(user, employeResquest.Password).Result;

        if (!result.Succeeded) 
            return Results.ValidationProblem(result.Errors.ConvertToProblemDetails());

        var userClaims = new List<Claim>
        {
            new Claim("EmployeeCode", employeResquest.EmployeeCode),
            new Claim("Name", employeResquest.Name)
        };

        var claimResult = userManager.AddClaimsAsync(user, userClaims).Result;

        if (!claimResult.Succeeded)
            return Results.BadRequest(claimResult.Errors.First());



        return Results.Created($"/employees/{user.Id}", user.Id);
    }

}

