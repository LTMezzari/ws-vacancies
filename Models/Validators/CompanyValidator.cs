using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;

namespace ws_vacancies.Models.Validators {
    public class CompanyValidator: AbstractValidator<Company> {
        public CompanyValidator() {
            RuleFor(c => c.Name)
                .NotNull()
                .WithMessage("O nome da empresa é obrigatório")
                .NotEmpty()
                .WithMessage("O nome da empresa é obrigatório");
        }
    }
}