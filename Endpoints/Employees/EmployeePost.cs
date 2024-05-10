
using IWantApp.Domain.Users;
using IWantApp.Endpoints;
using IWantApp.Endpoints.Clients;
using IWantApp.Endpoints.Employees;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;



public class EmployeesPost
{
    public static string Template => "/employees";

    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };

    public static Delegate Handle => Action;

    [Authorize(Policy = "EmployeePolicy")]
    public static async Task<IResult> Action(EmployeResquest employeResquest, UserCreator userCreator, HttpContext http)
    {
        var userId = http.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value; 
        var userClaims = new List<Claim>
        {
            new Claim("EmployeeCode", employeResquest.EmployeeCode),
            new Claim("Name", employeResquest.Name),
            new Claim("CreatedBy", userId)
        };

        (IdentityResult identity, string userId) result =
            await userCreator.Create(employeResquest.Email, employeResquest.Password, userClaims);

        if (!result.identity.Succeeded)
        {
            return Results.ValidationProblem(result.identity.Errors.ConvertToProblemDetails());
        }




        return Results.Created($"/employees/{result.userId}", result.userId);
    }

}
