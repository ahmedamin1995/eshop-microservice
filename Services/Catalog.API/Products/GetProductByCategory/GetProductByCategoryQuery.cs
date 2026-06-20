

namespace Catalog.API.Products.GetProductByCategory;

public record GetProductByCategoryResult(IEnumerable<Product> Products);
public record GetProductByCategoryQuery(string Category):IQuery<GetProductByCategoryResult>;


internal class GetProductByCategoryQueryHandler(ILogger<GetProductByCategoryQueryHandler> logger, IDocumentSession session)
    : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryResult>
{
    public async Task<GetProductByCategoryResult> Handle(GetProductByCategoryQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling GetProductByCategoryQuery called {@request}", query);

        var products = await session.Query<Product>()
                    .Where(x => x.Category.Contains(query.Category))
                    .ToListAsync(cancellationToken);


        return new GetProductByCategoryResult(products);
    }
}