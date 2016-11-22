using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Dispatcher;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Routing;
using log4net.Config;
using SDesk.Web.WebApi.Constraints;
using SDesk.Web.WebApi.Filters;

namespace SDesk.Web.WebApi
{
    /// <summary>
    /// 
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public static void Register(HttpConfiguration config)
        {
            // log4net configuration
            XmlConfigurator.Configure();
            config.Services.Add(typeof(IExceptionLogger), new ExceptionLogger());

            //config.Services.Replace(typeof(IHttpControllerSelector), new CustomControllerSelector((config)));
            var apiExplorer = config.Services.GetApiExplorer();
            config.Services.Replace(typeof(IApiExplorer), new VersionedApiExplorer<VersionConstraint>(apiExplorer, config));

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