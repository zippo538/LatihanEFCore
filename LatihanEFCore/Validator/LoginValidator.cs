using FluentValidation;
using LatihanEFCore.DTOs;
namespace LatihanEFCore.Validator
{
    public class LoginValidator : AbstractValidator<LoginDto>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is Required")
            .EmailAddress().WithMessage("Please Provide Email Addess");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required");
        }
    }
}