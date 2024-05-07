
using IWantApp.Endpoints;
using IWantApp.Endpoints.Employees;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;


public class EmployeesPost
{
    public static string Template => "/employees";

    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };

    public static Delegate Handle => Action;

    public static async Task<IResult> Action(EmployeResquest employeResquest, UserManager<IdentityUser> userManager, HttpContext http)
    {

       var userId = http.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
       var newUser = new IdentityUser { UserName = employeResquest.Email, Email = employeResquest.Email };
       var result = await userManager.CreateAsync(newUser, employeResquest.Password);

        if (!result.Succeeded) 
            return Results.ValidationProblem(result.Errors.ConvertToProblemDetails());

        var userClaims = new List<Claim>
        {
            new Claim("EmployeeCode", employeResquest.EmployeeCode),
            new Claim("Name", employeResquest.Name),
            new Claim("CreatedBy", userId)
        };

        var claimResult = await userManager.AddClaimsAsync(newUser, userClaims);

        if (!claimResult.Succeeded)
            return Results.BadRequest(claimResult.Errors.First());



        return Results.Created($"/employees/{newUser.Id}", newUser.Id);
    }

}

