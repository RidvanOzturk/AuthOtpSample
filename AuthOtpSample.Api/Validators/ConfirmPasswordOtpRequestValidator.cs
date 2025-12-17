using AuthOtpSample.Api.Models.Request;
using FluentValidation;

namespace AuthOtpSample.Api.Validators;

public sealed class ConfirmPasswordOtpRequestValidator : AbstractValidator<ConfirmPasswordOtpRequest>
{
    public ConfirmPasswordOtpRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Otp)
            .NotEmpty()
            .Length(6);

        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .MinimumLength(5);
    }
}