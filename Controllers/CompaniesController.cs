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

namespace ws_vacancies.Controllers {
    public class CompaniesController: ApiController {
        private DbVacanciesContext Database = new DbVacanciesContext();
        private CompanyValidator Validator = new CompanyValidator();

        //POST => api/Companies
        [ResponseType(typeof(Company))]
        public IHttpActionResult PostCompany(Company company) {
            try {
                if (!ModelState.IsValid) {
                    return BadRequest(ModelState);
                }

                Validator.ValidateAndThrow(company);

                Database.Companies.Add(company);
                Database.SaveChanges();

                return CreatedAtRoute("DefaultApi", new { Id = company.Id }, company);
            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }

        //GET => api/Companies
        [ResponseType(typeof(List<Company>))]
        public IHttpActionResult GetCompany(int page = 1, int offset = 10) {
            try {
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

                var companies = Database.Companies.OrderBy(c => c.Name).Skip(offset * (page - 1)).Take(offset);
                return Ok(companies);
            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }

        //GET => api/Companies/:id
        [ResponseType(typeof(Company))]
        public IHttpActionResult GetCompany(int id) {
            try {
                if (id < 0) {
                    return BadRequest("Invalid Id");
                }

                var company = Database.Companies.Find(id);
                if (company == null)
                    return NotFound();

                return Ok(company);
            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }

        //PUT => api/Companies/:id
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCompany(int id, Company company) {
            try {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                Validator.ValidateAndThrow(company);

                if (id < 0) {
                    return BadRequest("Invalid Id");
                } else if (company.Id != id) {
                    return BadRequest("Mismatching Id's");
                }

                if (Database.Companies.Count(c => c.Id == id) == 0)
                    return NotFound();

                Database.Entry(company).State = System.Data.Entity.EntityState.Modified;
                Database.SaveChanges();

                return StatusCode(HttpStatusCode.NoContent);
            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }

        //DELETE => api/Companies/:id
        [ResponseType(typeof(void))]
        public IHttpActionResult DeleteCompany(int id) {
            try {
                if (id < 0) {
                    return BadRequest("Invalid Id");
                }

                var company = Database.Companies.Find(id);
                if (company == null)
                    return NotFound();

                if (company.Vacancies != null) {
                    company.Vacancies.ToList().ForEach(v => {
                        if (v.Requirements != null)
                            Database.Requirements.RemoveRange(v.Requirements);
                    });
                    Database.Vacancies.RemoveRange(company.Vacancies);
                }
                if (company.Announcements != null) {
                    company.Vacancies.ToList().ForEach(v => {
                        if (v.Requirements != null)
                            Database.Requirements.RemoveRange(v.Requirements);
                    });
                    Database.Vacancies.RemoveRange(company.Announcements);
                }
                Database.Companies.Remove(company);
                Database.SaveChanges();

                return StatusCode(HttpStatusCode.NoContent);
            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }
    }
}