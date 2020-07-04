using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ws_vacancies.Models {
    public class Requirement {
        public int Id { get; set; }

        public string Description { get; set; }

        [JsonIgnore]
        public int VacancyId { get; set; }

        [JsonIgnore]
        public virtual Vacancy Vacancy { get; set; }
    }
}