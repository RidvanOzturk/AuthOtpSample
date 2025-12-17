using AuthOtpSample.Api.Models.Request;
using FluentValidation;

namespace AuthOtpSample.Api.Validators;

public sealed class UpdateProfileRequestValidator : AbstractValidator<UpdateProfileRequest>
{
    public UpdateProfileRequestValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(150);

        RuleFor(x => x.Surname)
            .MaximumLength(150);
    }
}