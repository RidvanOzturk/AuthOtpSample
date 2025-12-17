using AuthOtpSample.Api.Models.Request;
using FluentValidation;

namespace AuthOtpSample.Api.Validators;

public sealed class ConfirmOtpRequestValidator : AbstractValidator<ConfirmOtpRequest>
{
    public ConfirmOtpRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Otp)
            .NotEmpty()
            .Length(6);
    }
}