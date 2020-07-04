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

namespace ws_vacancies.Controllers
{
    public class RequirementsController : ApiController {
        private DbVacanciesContext Database = new DbVacanciesContext();
        private RequirementValidator Validator = new RequirementValidator();

        //POST => api/Companies/:companyId/Vacancies/:vacancyId/Requirements
        [ResponseType(typeof(Requirement))]
        public IHttpActionResult PostRequirement(int companyId, int vacancyId, Requirement requirement) {
            try {
                if (!ModelState.IsValid) {
                    return BadRequest(ModelState);
                }

                var company = Database.Companies.Find(companyId);
                if (company == null)
                    return NotFound();

                var vacancy = Database.Vacancies.Find(vacancyId);
                if (vacancy == null)
                    return NotFound();

                requirement.Vacancy = vacancy;
                requirement.VacancyId = vacancyId;

                Validator.ValidateAndThrow(requirement);

                Database.Requirements.Add(requirement);
                Database.SaveChanges();

                return CreatedAtRoute("DefaultApi", new { Id = requirement.Id }, requirement);
            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }

        //GET => api/Companies/:companyId/Vacancies/:vacancyId/Requirements/:id
        [ResponseType(typeof(Requirement))]
        public IHttpActionResult GetRequirement(int companyId, int vacancyId, int id) {
            try {
                var company = Database.Companies.Find(companyId);
                if (company == null)
                    return NotFound();

                var vacancy = Database.Vacancies.Find(vacancyId);
                if (vacancy == null)
                    return NotFound();

                var requirement = Database.Requirements.Find(id);
                if (requirement == null)
                    return NotFound();

                return Ok(requirement);
            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }

        //GET => api/Companies/:companyId/Vacancies/:vacancyId/Requirements
        [ResponseType(typeof(List<Requirement>))]
        public IHttpActionResult GetRequirement(int companyId, int vacancyId, int page = 1, int offset = 10) {
            try {
                var company = Database.Companies.Find(companyId);
                if (company == null)
                    return NotFound();

                var vacancy = Database.Vacancies.Find(vacancyId);
                if (vacancy == null)
                    return NotFound();

                int totalPages = Database.Companies.Count() / offset;

                System.Web.HttpContext.Current.Response.AddHeader("X-Pagination-TotalPages", totalPages.ToString());

                if (page > 1) {
                    System.Web.HttpContext.Current.Response.AddHeader("X-Pagination-PreviousPage",
                        Url.Link("DefaultApi", new { page = page - 1, offset = offset }));
                }

                if (page < totalPages) {
                    System.Web.HttpContext.Current.Response.AddHeader("X-Pagination-NextPage",
                        Url.Link("DefaultApi", new { page = page + 1, offset = offset }));
                }

                var requirements = Database.Requirements.OrderBy(c => c.Description)
                    .Skip(offset * (page - 1))
                    .Take(offset)
                    .ToList()
                    .FindAll(r => r.VacancyId == vacancyId);
                return Ok(requirements);
            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }

        //PUT => api/Companies/:companyId/Vacancies/:vacancyId/Requirements/:id
        [ResponseType(typeof(void))]
        public IHttpActionResult PutRequirement(int companyId, int vacancyId, int id, Requirement requirement) {
            try {
                if (!ModelState.IsValid) {
                    return BadRequest(ModelState);
                }

                var company = Database.Companies.Find(companyId);
                if (company == null)
                    return NotFound();

                var vacancy = Database.Vacancies.Find(vacancyId);
                if (vacancy == null)
                    return NotFound();

                if (id != requirement.Id) {
                    return BadRequest("Mismatching Id's");
                } else if (Database.Requirements.Count(r => r.Id == id) != 1) {
                    return NotFound();
                }

                requirement.Vacancy = vacancy;
                requirement.VacancyId = vacancyId;

                Validator.ValidateAndThrow(requirement);

                Database.Entry(requirement).State = System.Data.Entity.EntityState.Modified;
                Database.SaveChanges();

                return StatusCode(HttpStatusCode.NoContent);
            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }

        //DELETE => api/Companies/:companyId/Vacancies/:vacancyId/Requirements/:id
        [ResponseType(typeof(void))]
        public IHttpActionResult DeleteRequirement(int companyId, int vacancyId, int id) {
            try {
                var company = Database.Companies.Find(companyId);
                if (company == null)
                    return NotFound();

                var vacancy = Database.Vacancies.Find(vacancyId);
                if (vacancy == null)
                    return NotFound();

                var requirement = Database.Requirements.Find(id);
                if (requirement == null) {
                    return NotFound();
                }

                Database.Requirements.Remove(requirement);
                Database.SaveChanges();

                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (Exception e) {
                return BadRequest(e.Message);
            }
        }

    }
}
