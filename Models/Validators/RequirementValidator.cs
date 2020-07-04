using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;

namespace ws_vacancies.Models.Validators {
    public class RequirementValidator: AbstractValidator<Requirement> {
        public RequirementValidator() {
            RuleFor(r => r.Description)
                .NotNull()
                .WithMessage("A descrição do requisito é obrigatório")
                .NotEmpty()
                .WithMessage("A descrição do requisito é obrigatório");
        }
    }
}