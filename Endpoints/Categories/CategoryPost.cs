using IWantApp.Endpoints.Categories.Infra.Data;

namespace IWantApp.Endpoints.Categories;

public class CategoryPost
{
    public static string Template => "/categories";

    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };

    public static Delegate Handle => Action;

    public static IResult Action(CategoryRequest categoryRequest, ApplicationDbContext context)
    {
        var category = new Domain.Products.Category
        {
         Name = categoryRequest.Name,
         CreatedBy = "test", 
         CreatedOn = DateTime.Now,
         EditedBy = "Test",
         EditedOn = DateTime.Now,
        };

        context.Categories.Add(category);
        context.SaveChanges();

        return Results.Created($"/categories/{category.Id}", category.Id);
    }

}

