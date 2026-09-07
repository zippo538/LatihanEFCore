
using FluentValidation;
using LatihanEFCore.DTOs;

namespace LatihanEFCore.Validator
{
    public class CreateStudentValidator : AbstractValidator<CreateStudentDTO>
    {
        public CreateStudentValidator()
        {
            RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Name is required")
            .MaximumLength(100)
            .MinimumLength(5)
            .NotNull()
            .NotEqual("")
            .WithMessage("Name cannot exceed 100 characters");

            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Email is required")
                .NotEqual("")
                .EmailAddress()
                .WithMessage("Email format is invalid");

            RuleFor(x => x.PhoneNumber)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Phone number is required")
                .MaximumLength(20)
                .NotEqual("")
                .MinimumLength(5)
                .WithMessage("Phone number cannot exceed 20 characters");

            RuleFor(x => x.Address)
                .NotEmpty()
                .MinimumLength(5)
                .NotEqual("")
                .WithMessage("Address is required");


            RuleFor(x => x.IdOrganization)
                .GreaterThan(0)
                .WithMessage("IdOrganization must be greater than zero");
        }
    }
}