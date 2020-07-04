using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ws_vacancies.Models {
    public class Company {
        public int Id { get; set; }

        public string Name { get; set; }

        [JsonIgnore]
        public virtual ICollection<Vacancy> Vacancies { get; set; }

        [JsonIgnore]
        public virtual ICollection<Vacancy> Announcements { get; set; }
    }
}