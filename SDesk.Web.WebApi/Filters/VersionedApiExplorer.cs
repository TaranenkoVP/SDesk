﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Description;
using System.Web.Http.Routing;

namespace SDesk.Web.WebApi.Filters
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TVersionConstraint"></typeparam>
    public class VersionedApiExplorer<TVersionConstraint> : IApiExplorer
    {
        private IApiExplorer _innerApiExplorer;
        private HttpConfiguration _configuration;
        private Lazy<Collection<ApiDescription>> _apiDescriptions;
        private MethodInfo _apiDescriptionPopulator;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiExplorer"></param>
        /// <param name="configuration"></param>
        public VersionedApiExplorer(IApiExplorer apiExplorer, HttpConfiguration configuration)
        {
            _innerApiExplorer = apiExplorer;
            _configuration = configuration;
            _apiDescriptions = new Lazy<Collection<ApiDescription>>(
                new Func<Collection<ApiDescription>>(Init));
        }

        Collection<ApiDescription> IApiExplorer.ApiDescriptions
        {
            get { return _apiDescriptions.Value; }
        }

        private Collection<ApiDescription> Init()
        {
            var descriptions = _innerApiExplorer.ApiDescriptions;

            var controllerSelector = _configuration.Services.GetHttpControllerSelector();
            var controllerMappings = controllerSelector.GetControllerMapping();

            var flatRoutes = FlattenRoutes(_configuration.Routes);
            var result = new Collection<ApiDescription>();

            foreach (var description in descriptions)
            {
                result.Add(description);

                if (controllerMappings != null && description.Route.Constraints.Any(c => c.Value is TVersionConstraint))
                {
                    var matchingRoutes = flatRoutes.Where(r => r.RouteTemplate == description.Route.RouteTemplate && r != description.Route);

                    foreach (var route in matchingRoutes)
                        GetRouteDescriptions(route, result);
                }
            }
            return result;
        }

        private void GetRouteDescriptions(IHttpRoute route, Collection<ApiDescription> apiDescriptions)
        {
            var actionDescriptor = route.DataTokens["actions"] as IEnumerable<HttpActionDescriptor>;

            if (actionDescriptor != null && actionDescriptor.Count() > 0)
                GetPopulateMethod().Invoke(_innerApiExplorer, new object[] { actionDescriptor.First(), route, route.RouteTemplate, apiDescriptions });
        }

        private MethodInfo GetPopulateMethod()
        {
            if (_apiDescriptionPopulator == null)
                _apiDescriptionPopulator = _innerApiExplorer.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).FirstOrDefault(
                   m => m.Name == "PopulateActionDescriptions" && m.GetParameters().Length == 4);

            return _apiDescriptionPopulator;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="routes"></param>
        /// <returns></returns>
        public static IEnumerable<IHttpRoute> FlattenRoutes(IEnumerable<IHttpRoute> routes)
        {
            var flatRoutes = new List<HttpRoute>();

            foreach (var route in routes)
            {
                if (route is HttpRoute)
                    yield return route;

                var subRoutes = route as IReadOnlyCollection<IHttpRoute>;
                if (subRoutes != null)
                    foreach (IHttpRoute subRoute in FlattenRoutes(subRoutes))
                        yield return subRoute;
            }
        }
    }
}