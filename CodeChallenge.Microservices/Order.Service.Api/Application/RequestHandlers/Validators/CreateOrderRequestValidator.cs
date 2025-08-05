using FluentValidation;
using Order.Service.Shared.Request;

namespace Order.Service.Api.Application.RequestHandlers.Validators;

public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
{
    public CreateOrderRequestValidator()
    {
        RuleFor(x => x.InvoiceAddress)
            .NotEmpty().WithMessage("Invoice address is required.");

        RuleFor(x => x.InvoiceEmail)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Email address is required.")
            .EmailAddress().WithMessage("A valid email address is required.");

        RuleFor(x => x.CreditCardNumber)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Credit card number is required.")
            .Matches(@"^\d{4}-\d{4}-\d{4}-\d{4}$")
            .WithMessage("Credit card number must be in format ####-####-####-####.");

        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("At least one order item is required.");

        RuleForEach(x => x.Items).SetValidator(new OrderItemDtoValidator());
    }
}