using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;

namespace ws_vacancies.Models.Mapping {
    public class RequirementMapping: EntityTypeConfiguration<Requirement> {
        public RequirementMapping() {
            ToTable("tb_requirement");

            HasKey(r => r.Id);

            Property(r => r.Description)
                .IsRequired()
                .HasMaxLength(100);

            HasRequired<Vacancy>(r => r.Vacancy)
                .WithMany(v => v.Requirements)
                .HasForeignKey(v => v.VacancyId)
                .WillCascadeOnDelete(false);
        }
    }
}