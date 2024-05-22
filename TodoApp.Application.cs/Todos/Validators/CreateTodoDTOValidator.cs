using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Application.cs.Todos.DTOs;

namespace TodoApp.Application.cs.Todos.Validators
{
    public class CreateTodoDTOValidator : AbstractValidator<CreateTodoDTO>
    {
        public CreateTodoDTOValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(ValidationMessages.EMPTY_STRING_ERROR_MESSAGE)
                .MinimumLength(5).WithMessage(ValidationMessages.MIN_LENGTH_ERROR_MESSAGE)
                .MaximumLength(30).WithMessage(ValidationMessages.MAX_LENGTH_ERROR_MESSAGE);
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage(ValidationMessages.EMPTY_STRING_ERROR_MESSAGE)
                .MinimumLength(5).WithMessage(ValidationMessages.MIN_LENGTH_ERROR_MESSAGE)
                .MaximumLength(100).WithMessage(ValidationMessages.MAX_LENGTH_ERROR_MESSAGE);
            RuleFor(x => x.PrevisionDate)
                .GreaterThan(DateTime.Now.AddDays(-1)).WithMessage(ValidationMessages.ERROR_MESSAGE);
        }
    }
}
