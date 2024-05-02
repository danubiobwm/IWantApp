
using IWantApp.Infra.Data;

namespace IWantApp.Endpoints.Categories;

public class CategoryPost
{
    public static string Template => "/categories";

    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };

    public static Delegate Handle => Action;

    public static IResult Action(CategoryRequest categoryRequest, ApplicationDbContext context)
    {
        var category = new Domain.Products.Category(categoryRequest.Name, "test", "Test");
      

        if (!category.IsValid) 
        {
            var erros = category.Notifications
                .GroupBy(g => g.Key)
                .ToDictionary(g=>g.Key, g => g.Select(x => x.Message).ToArray());
            return Results.ValidationProblem(erros);

        }

        context.Categories.Add(category);
        context.SaveChanges();

        return Results.Created($"/categories/{category.Id}", category.Id);
    }

}

