using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http.Routing;

namespace SDesk.Web.WebApi.Constraints
{
    public class VersionConstraint : IHttpRouteConstraint
    {
        public bool Match(HttpRequestMessage request, IHttpRoute route, string parameterName,
            IDictionary<string, object> values, HttpRouteDirection routeDirection)
        {
            return request.Headers.Contains("api-version");
        }
    }
}