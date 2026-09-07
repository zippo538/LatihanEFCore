using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using LatihanEFCore.DTOs;

namespace LatihanEFCore.Validator.TeacherValidator
{
    public class UpdateTeacherValidator : AbstractValidator<UpdateTeacherDto>
    {
        public UpdateTeacherValidator()
        {
            RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Email must written");
            RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone Number is Required")
            .MaximumLength(15).WithMessage("Phone Number Cannot exeed 15 number");
            RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address must written");
            RuleFor(x => x.IdCourse)
                .NotEmpty().WithMessage("IdCourse must filled");
        }
    }
}