using FluentValidation;
using Order.Service.Shared.Request;

namespace Order.Service.Api.Application.RequestHandlers.Validators;

public class DeleteOrderRequestValidator : AbstractValidator<DeleteOrderRequest>
{
    public DeleteOrderRequestValidator()
    {
        RuleFor(q => q.OrderNumber).NotEmpty().WithMessage("OrderNumber is required.");
    }
}