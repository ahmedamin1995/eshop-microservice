namespace Catalog.API.Products.GetProductById;

public record GetProductByIdResponse(Product Product);
public class GetProductByIdEndpoint:CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/{id:guid}", async (IMediator mediator, Guid id, CancellationToken cancellationToken) =>
        {
            var query = new GetProductByIdQuery(id);
            var result = await mediator.Send(query, cancellationToken);
            var response= result.Adapt<GetProductByIdResponse>();
            return Results.Ok(response);
        })
        .WithName("GetProductById")
        .Produces<GetProductByIdResult>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError);
    }
}

