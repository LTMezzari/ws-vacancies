using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;

namespace ws_vacancies.Models.Validators {
    public class VacancyValidator: AbstractValidator<Vacancy> {
        public VacancyValidator() {
            RuleFor(v => v.Title)
                .NotNull()
                .WithMessage("O título da vaga é obrigatório")
                .NotEmpty()
                .WithMessage("O título da vaga é obrigatório")
                .Length(10, 100)
                .WithMessage("O título da vaga deve ser entre {MinLength} {MaxLength}");

            RuleFor(v => v.Description)
                .NotNull()
                .WithMessage("A descrição da vaga é obrigatório")
                .NotEmpty()
                .WithMessage("A descrição da vaga é obrigatório")
                .Length(10, 200)
                .WithMessage("A descrição da vaga deve ser entre {MinLength} {MaxLength}");

            RuleFor(v => v.Salary)
                .NotNull()
                .WithMessage("O salário da vaga é obrigatório")
                .NotEmpty()
                .WithMessage("O salário da vaga é obrigatório")
                .GreaterThan(0)
                .WithMessage("O salário da vaga deve ser maior que {ComparisonValue}");

            RuleFor(v => v.IsActive)
                .NotNull()
                .WithMessage("O status da vaga é obrigatório");

            RuleFor(v => v.RegisteredDate)
                .NotNull()
                .WithMessage("A data de registro da vaga é obrigatória");

            RuleFor(v => v.WorkPlace)
                .NotNull()
                .WithMessage("O local de trabalho da vaga é obrigatório")
                .NotEmpty()
                .WithMessage("O local de trabalho da vaga é obrigatório")
                .Length(10, 200)
                .WithMessage("O local de trabalho da vaga deve ser entre {MinLength} {MaxLength}");

            RuleFor(v => v.Company)
                .NotNull()
                .WithMessage("A empresa da vaga é obrigatória")
                .SetValidator(new CompanyValidator());

            RuleFor(v => v.Announcer)
                .NotNull()
                .WithMessage("O anunciante da vaga é obrigatório")
                .SetValidator(new CompanyValidator());

            RuleForEach(v => v.Requirements)
                .NotNull()
                .WithMessage("Os requisitos da vaga são obrigatórios")
                .SetValidator(new RequirementValidator());

            RuleFor(v => v.Requirements.Count)
                .GreaterThanOrEqualTo(1)
                .WithMessage("Uma vaga deve ter no mínimo {ComparisonValue} requisito");

            RuleFor(v => v)
                .Must(v => v.Announcer.Id != v.Company.Id)
                .WithMessage("A empresa e o anunciante devem ser diferentes");

            When(v => v.Salary <= 1700, () => {
                RuleFor(v => v.Requirements.Count)
                    .LessThanOrEqualTo(3)
                    .WithMessage("Uma vaga deve ter no máximo {ComparisonValue} requisito")
                    .GreaterThanOrEqualTo(1)
                    .WithMessage("Uma vaga deve ter no mínimo {ComparisonValue} requisito");
            });
        }
    }
}