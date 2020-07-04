using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ws_vacancies.Models {
    public class Vacancy {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public decimal Salary { get; set; }

        public bool IsActive { get; set; }

        public DateTime RegisteredDate { get; set; }

        public string WorkPlace { get; set; }

        [JsonIgnore]
        public int CompanyId { get; set; }

        [JsonIgnore]
        public virtual Company Company { get; set; }

        public int AnnouncerId { get; set; }

        [JsonIgnore]
        public virtual Company Announcer { get; set; }

        public virtual ICollection<Requirement> Requirements { get; set; }

        public Vacancy() {
            this.IsActive = true;
            this.RegisteredDate = DateTime.Now;
        }
    }
}