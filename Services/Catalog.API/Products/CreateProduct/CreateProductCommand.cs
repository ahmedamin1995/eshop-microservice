namespace Catalog.API.Products.CreateProduct;


public class CreateProductCommandValidator:AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
        RuleFor(x => x.Category).NotEmpty().WithMessage("Category is required.");
        RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required.");
        RuleFor(x => x.ImageFile).NotEmpty().WithMessage("Image file is required.");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than zero.");
    }
}


public record CreateProductCommand(
    string Name,
    List<string> Category,
    string Description,
    string ImageFile,
    decimal Price
) : ICommand<CreateProductResult>;

public record CreateProductResult(Guid Id);

internal class CreateProductCommandHandler(IDocumentSession session) : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        Product product = CreateProduct(command);

        // Rest of the method implementation

        session.Store(product);
         await session.SaveChangesAsync(cancellationToken);

        return new CreateProductResult(product.Id);
    }

    private Product CreateProduct(CreateProductCommand command)
    {
        return new Product()
        {
            Category = command.Category,
            Description = command.Description,
            ImageFile = command.ImageFile,
            Name = command.Name,
            Price = command.Price,
            Id = Guid.NewGuid()
        };
    }
}

