using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Routing;
using log4net.Config;
using SDesk.Web.WebApi.Constraints;

namespace SDesk.Web.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // log4net configuration
            XmlConfigurator.Configure();
            config.Services.Add(typeof(IExceptionLogger), new ExceptionLogger());

            var constraintResolver = new DefaultInlineConstraintResolver();
            constraintResolver.ConstraintMap.Add("jiraid", typeof(JiraIdConstraint));
            config.MapHttpAttributeRoutes(constraintResolver);

            config.Routes.MapHttpRoute(
                name: "JiraIdConstraint",
                routeTemplate: "api/jiraitems/{id}",
                defaults: new { controller = "jiraitems" },
                constraints: new { id = new JiraIdConstraint() });

            config.Routes.MapHttpRoute(
                "DefaultApi",
                "api/{controller}/{id}",
                new {id = RouteParameter.Optional});
        }
    }
}