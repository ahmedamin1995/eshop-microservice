using OpenTelemetry.Trace;

namespace Catalog.API.Products.GetProducts
{
    public record GetProductsResponse(GetProductsResult Products);

    public class GetProductsEndpoint:ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products", async (IMediator mediator, CancellationToken cancellationToken) =>
            {
                var query = new GetProductsQuery();
                var result = await mediator.Send(query, cancellationToken);
                return Results.Ok(new GetProductsResponse(result));
            })
            .WithName("GetProducts")
            .Produces<GetProductsResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);
        }
    }
   
}
