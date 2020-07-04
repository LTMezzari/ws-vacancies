using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ws_vacancies.Models;
using ws_vacancies.Models.Database;
using ws_vacancies.Models.Validators;
using FluentValidation;
using System.Web.Http.Description;
using System.Data.Entity;

namespace ws_vacancies.Controllers {
    public class VacanciesController: ApiController {
        private DbVacanciesContext Database = new DbVacanciesContext();
        private VacancyValidator Validator = new VacancyValidator();

        //POST => api/Companies/:companyId/Vacancies}
        [ResponseType(typeof(Vacancy))]
        public IHttpActionResult PostVacancy(int companyId, Vacancy vacancy) {
            try {
                if (!ModelState.IsValid) {
                    return BadRequest(ModelState);
                }

                var company = Database.Companies.Find(companyId);
                if (company == null)
                    return NotFound();

                vacancy.Company = company;
                vacancy.CompanyId = company.Id;

                var announcer = Database.Companies.Find(vacancy.AnnouncerId);
                if (announcer == null)
                    return NotFound();

                vacancy.Announcer = announcer;

                Validator.ValidateAndThrow(vacancy);
                Database.Vacancies.Add(vacancy);

                vacancy.Requirements.ToList().ForEach(r => {
                    r.VacancyId = vacancy.Id;
                    r.Vacancy = vacancy;

                    Database.Requirements.Add(r);
                });

                Database.SaveChanges();

                return CreatedAtRoute("VacanciesApi", new { Id = vacancy.Id }, vacancy);
            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }

        //GET => api/Companies/:companyId/Vacancies/:id}
        [ResponseType(typeof(Vacancy))]
        public IHttpActionResult GetVacancy(int companyId, int id) {
            try {
                var company = Database.Companies.Find(companyId);
                if (company == null)
                    return NotFound();

                var vacancy = Database.Vacancies.Find(id);
                if (vacancy == null)
                    return NotFound();

                return Ok(vacancy);
            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }

        //GET => api/Companies/:companyId/Vacancies}
        [ResponseType(typeof(List<Vacancy>))]
        public IHttpActionResult GetVacancy(int companyId, int page = 1, int offset = 10) {
            try {
                var company = Database.Companies.Find(companyId);
                if (company == null)
                    return NotFound();

                int totalPages = Database.Vacancies.Count() / offset;

                System.Web.HttpContext.Current.Response.AddHeader("X-Pagination-TotalPages", totalPages.ToString());

                if (page > 1) {
                    System.Web.HttpContext.Current.Response.AddHeader("X-Pagination-PreviousPage",
                        Url.Link("VacanciesApi", new { companyId = companyId, page = page - 1, offset = offset }));
                }

                if (page < totalPages) {
                    System.Web.HttpContext.Current.Response.AddHeader("X-Pagination-NextPage",
                        Url.Link("VacanciesApi", new { companyId = companyId, page = page + 1, offset = offset }));
                }

                var vacancies = Database.Vacancies.Include(v => v.Requirements)
                    .OrderBy(v => v.Id)
                    .Skip(offset * (page - 1))
                    .Take(offset)
                    .ToList()
                    .FindAll(v => v.IsActive && v.CompanyId == companyId);
                return Ok(vacancies);
            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }

        //PUT => api/Companies/:companyId/Vacancies/:id}
        [ResponseType(typeof(void))]
        public IHttpActionResult PutVacancy(int companyId, int id, Vacancy vacancy) {
            try {
                if (!ModelState.IsValid) {
                    return BadRequest(ModelState);
                }

                if (id < 0 || companyId < 0) {
                    return BadRequest("Invalid Id's");
                } else if (vacancy.Id != id) {
                    return BadRequest("Mismatching Id's");
                }

                var company = Database.Companies.Find(companyId);
                if (company == null)
                    return NotFound();

                if (Database.Vacancies.Count(v => v.Id == id) != 1)
                    return NotFound();

                vacancy.Company = company;
                vacancy.CompanyId = company.Id;

                var announcer = Database.Companies.Find(vacancy.AnnouncerId);
                if (announcer == null)
                    return NotFound();

                vacancy.Announcer = announcer;

                Validator.ValidateAndThrow(vacancy);

                vacancy.Requirements.ToList().ForEach(r => {
                    if (r.Id > 0)
                        return;

                    r.VacancyId = vacancy.Id;
                    r.Vacancy = vacancy;

                    Database.Requirements.Add(r);
                });

                Database.Entry(vacancy).State = System.Data.Entity.EntityState.Modified;
                Database.SaveChanges();

                return StatusCode(HttpStatusCode.NoContent);
            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }

        //PUT => api/Companies/:companyId/Vacancies}
        [ResponseType(typeof(void))]
        public IHttpActionResult DeleteVacancy(int companyId, int id) {
            try {
                var company = Database.Companies.Find(companyId);
                if (company == null)
                    return NotFound();

                var vacancy = Database.Vacancies.Find(id);
                if (vacancy == null)
                    return NotFound();

                Database.Requirements.RemoveRange(vacancy.Requirements);
                Database.Vacancies.Remove(vacancy);
                Database.SaveChanges();

                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (Exception e) {
                return BadRequest(e.Message);
            }
        }
    }
}