
using IWantApp.Domain.Orders;
using IWantApp.Domain.Products;
using IWantApp.Endpoints;
using IWantApp.Infra.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;




public class OrderPost
{
    public static string Template => "/orders";

    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };

    public static Delegate Handle => Action;

    [Authorize(Policy = "CpfPolicy")]
    public async static Task<IResult> Action(OrderResquest orderResquest, HttpContext http, ApplicationDbContext context)
    {
       var clientId = http.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
       var clientName = http.User.Claims.First(c => c.Type == "Name").Value;
       
       List<Product> productFound = null;

        if (orderResquest.ProductId != null || orderResquest.ProductId.Any())
            productFound = context.Products.Where(p=> orderResquest.ProductId.Contains(p.Id)).ToList();


        var order = new Order(clientId, clientName, productFound, orderResquest.DeliveryAddress);

        if (!order.IsValid)
            return Results.ValidationProblem(order.Notifications.ConvertToProblemDetails());


        await context.Orders.AddAsync(order);
        await context.SaveChangesAsync();

        return Results.Created($"/orders/{order.Id}", order.Id);
    }

}

