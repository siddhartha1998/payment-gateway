using FluentValidation;

namespace PaymentGateway.Server.Application.Payments.Commands
{
    public class CreatePaymentCommandValidator : AbstractValidator<CreatePaymentCommand>
    {
        public CreatePaymentCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email address.");

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than zero.");

            RuleFor(x => x.Currency)
                .NotEmpty().WithMessage("Currency is required.");

            RuleFor(x => x.PaymentMethod)
                .NotEmpty().WithMessage("Payment mode is required.");

            RuleFor(x => x.AccountNumber)
                .NotEmpty().WithMessage("Account number is required.")
                .When(x => x.PaymentMethod == Domain.Enums.PaymentMethod.BankTransfer);

            RuleFor(x => x.BankName)
                .NotEmpty().WithMessage("Bank name is required.")
                .When(x => x.PaymentMethod == Domain.Enums.PaymentMethod.BankTransfer);

            RuleFor(x => x.CardNumber)
                .NotEmpty().WithMessage("Card number is required.")
                .When(x => x.PaymentMethod == Domain.Enums.PaymentMethod.Card);

            RuleFor(x => x.ExpiryDate)
                .NotEmpty().WithMessage("Expiry date is required.")
                .When(x => x.PaymentMethod == Domain.Enums.PaymentMethod.Card);

            RuleFor(x => x.Cvv)
                .NotEmpty().WithMessage("CVV is required.")
                .When(x => x.PaymentMethod == Domain.Enums.PaymentMethod.Card);
        }
    }
}
