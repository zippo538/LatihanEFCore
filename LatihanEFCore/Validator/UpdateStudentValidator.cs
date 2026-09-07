using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using LatihanEFCore.DTOs;

namespace LatihanEFCore.Validator
{
    public class UpdateStudentValidator : AbstractValidator<UpdateStudentDTO>
    {
        public UpdateStudentValidator()
        {
            RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(25).WithMessage("Name cannot exceed 25 characters");
            RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Email must written");
            RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone Number is Required")
            .MaximumLength(15).WithMessage("Phone Number Cannot exeed 15 number");
            RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address must written");
            RuleFor(x => x.GPA)
            .NotEmpty().WithMessage("GPA must written")
            .LessThan(4).WithMessage("GPA must below 4.0 ");
        }
    }
}