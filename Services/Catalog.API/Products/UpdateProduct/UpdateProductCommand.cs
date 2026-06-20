using Catalog.API.Models;
using Catalog.API.Products.CreateProduct;

namespace Catalog.API.Products.UpdateProduct;

public record UpdateProductResult(bool IsSuccess);
public record UpdateProductCommand(Guid Id, string Name, List<string> Category, string Description, decimal Price, string ImageUrl) : ICommand<UpdateProductResult>;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Product ID is required.");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.")
            .Length(2, 150).WithMessage("Name Length between 2 and 150 char");
        RuleFor(x => x.Category).NotEmpty().WithMessage("Category is required.");
        RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required.");

        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than zero.");
    }
}

internal class UpdateProductCommandHandler(IDocumentSession session, ILogger<UpdateProductCommandHandler> logger) : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling UpdateProductCommand called {@command}", command);
        var product = await session.LoadAsync<Product>(command.Id, cancellationToken);
        if (product == null)
        {
            throw new ProductNotFoundException(command.Id);
        }

        product = UpdateProduct(command, product);

        session.Update(product);
        await session.SaveChangesAsync(cancellationToken);

        return new UpdateProductResult(true);
    }

    private Product UpdateProduct(UpdateProductCommand command, Product product)
    {
        product.Name = command.Name;
        product.Description = command.Description;
        product.Price = command.Price;
        product.ImageFile = command.ImageUrl;
        product.Category = command.Category;

        return product;
    }
}