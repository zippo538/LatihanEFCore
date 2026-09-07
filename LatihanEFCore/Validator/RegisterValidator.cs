using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using LatihanEFCore.DTOs;

namespace LatihanEFCore.Validator
{
    public class RegisterValidator : AbstractValidator<RegisterDTO>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Name is required")
            .MaximumLength(100)
            .WithMessage("Name cannot exceed 100 characters");

            RuleFor(x => x.Email)
               .NotEmpty().WithMessage("Email is required")
               .EmailAddress().WithMessage("Please provide a valid email address");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)")
                .WithMessage("Password must contain at least one lowercase letter, one uppercase letter, and one digit");
        }
    }
}