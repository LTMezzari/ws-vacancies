using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;

namespace ws_vacancies.Models.Mapping {
    public class VacancyMapping: EntityTypeConfiguration<Vacancy> {
        public VacancyMapping() {
            ToTable("tb_vacancy");

            HasKey(v => v.Id);

            Property(v => v.Title)
                .IsRequired()
                .HasMaxLength(100);

            Property(v => v.Description)
                .IsRequired()
                .HasMaxLength(200);

            Property(v => v.Salary)
                .IsRequired();

            Property(v => v.WorkPlace)
                .IsRequired()
                .HasMaxLength(100);

            Property(v => v.RegisteredDate)
                .IsRequired();

            Property(v => v.IsActive)
                .IsRequired();

            HasRequired<Company>(v => v.Company)
                .WithMany(c => c.Vacancies)
                .HasForeignKey(v => v.CompanyId)
                .WillCascadeOnDelete(false);

            HasRequired<Company>(v => v.Announcer)
                .WithMany(c => c.Announcements)
                .HasForeignKey(v => v.AnnouncerId)
                .WillCascadeOnDelete(false);
        }
    }
}