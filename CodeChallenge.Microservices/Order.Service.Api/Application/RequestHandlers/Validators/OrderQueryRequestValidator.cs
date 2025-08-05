using FluentValidation;
using Order.Service.Shared.Request;

namespace Order.Service.Api.Application.RequestHandlers.Validators;

public class OrderQueryRequestValidator : AbstractValidator<OrderQueryRequest>
{
    public OrderQueryRequestValidator()
    {
        RuleFor(q => q.OrderNumber).NotEmpty().WithMessage("OrderNumber is required.");
    }
}