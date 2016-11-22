using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Routing;

namespace SDesk.Web.WebApi.Constraints
{
    /// <summary>
    /// 
    /// </summary>
    public class VersionConstraint : IHttpRouteConstraint
    {
        /// <summary>
        /// 
        /// </summary>
        public const string VersionHeaderName = "api-version";
        private const int DefaultVersion = 1;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="allowedVersion"></param>
        public VersionConstraint(int allowedVersion)
        {
            AllowedVersion = allowedVersion;
        }
        /// <summary>
        /// 
        /// </summary>
        public int AllowedVersion
        {
            get;
            private set;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="route"></param>
        /// <param name="parameterName"></param>
        /// <param name="values"></param>
        /// <param name="routeDirection"></param>
        /// <returns></returns>
        public bool Match(HttpRequestMessage request, IHttpRoute route, string parameterName, IDictionary<string, object> values, HttpRouteDirection routeDirection)
        {
            if (routeDirection == HttpRouteDirection.UriResolution)
            {
                int version = GetVersionHeader(request) ?? DefaultVersion;
                if (version == AllowedVersion)
                {
                    return true;
                }
            }
            return false;
        }
        private int? GetVersionHeader(HttpRequestMessage request)
        {
            string versionAsString;
            IEnumerable<string> headerValues;
            if (request.Headers.TryGetValues(VersionHeaderName, out headerValues) && headerValues.Count() == 1)
            {
                versionAsString = headerValues.First();
            }
            else
            {
                return null;
            }
            int version;
            if (versionAsString != null && Int32.TryParse(versionAsString, out version))
            {
                return version;
            }
            return null;
        }
    }
}