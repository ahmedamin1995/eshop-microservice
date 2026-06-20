


using Catalog.API.Products.GetProductById;

namespace Catalog.API.Products.GetProductByCategory;

public record GetProductByCategoryResponse(IEnumerable<Product> Products);
public class GetProductByCategoryEndpoint : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/category/{category}", async (ISender sender, string category, CancellationToken cancellationToken) =>
        {
            var query = new GetProductByCategoryQuery(category);

            var result= await sender.Send(query, cancellationToken);

            var response = result.Adapt<GetProductByCategoryResponse>();

            return Results.Ok(response);
        }).WithName("GetProductByCategory")
        .Produces<GetProductByIdResult>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError);
    }
}
