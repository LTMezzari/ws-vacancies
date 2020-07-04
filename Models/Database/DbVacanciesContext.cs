using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using ws_vacancies.Models.Mapping;

namespace ws_vacancies.Models.Database {
    public class DbVacanciesContext: DbContext {
        public DbSet<Company> Companies { get; set; }
        
        public DbSet<Vacancy> Vacancies { get; set; }
        
        public DbSet<Requirement> Requirements { get; set; }

        public DbVacanciesContext(): base("VacanciesContext") {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Configurations.Add(new CompanyMapping());
            modelBuilder.Configurations.Add(new VacancyMapping());
            modelBuilder.Configurations.Add(new RequirementMapping());
        }
    }
}