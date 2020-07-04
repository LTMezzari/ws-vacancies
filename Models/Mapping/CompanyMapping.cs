using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;

namespace ws_vacancies.Models.Mapping {
    public class CompanyMapping: EntityTypeConfiguration<Company> {
        public CompanyMapping() {
            ToTable("tb_company");

            HasKey(c => c.Id);

            Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}