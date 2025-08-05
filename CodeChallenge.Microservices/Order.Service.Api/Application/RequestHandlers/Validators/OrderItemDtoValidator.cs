using FluentValidation;
using Order.Service.Shared.Model;

namespace Order.Service.Api.Application.RequestHandlers.Validators;

public class OrderItemDtoValidator : AbstractValidator<OrderItemDto>
{
    public OrderItemDtoValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Product ID is required.");

        RuleFor(x => x.ProductName)
            .NotEmpty().WithMessage("Product name is required.");

        RuleFor(x => x.ProductAmount)
            .GreaterThan(0).WithMessage("Product amount must be greater than 0.");

        RuleFor(x => x.ProductPrice)
            .GreaterThan(0).WithMessage("Product price must be greater than 0.");
    }
}