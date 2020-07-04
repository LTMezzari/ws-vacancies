using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ws_vacancies
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Serviços e configuração da API da Web

            // Rotas da API da Web
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "VacanciesApi",
                routeTemplate: "api/Companies/{companyId}/{controller}/{id}",
                defaults: new {
                    controller = "Vacancies",
                    id = RouteParameter.Optional
                }
            );

            config.Routes.MapHttpRoute(
                name: "RequirementsApi",
                routeTemplate: "api/Companies/{companyId}/Vacancies/{vacancyId}/{controller}/{id}",
                defaults: new {
                    controller = "Requirements",
                    id = RouteParameter.Optional
                }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
