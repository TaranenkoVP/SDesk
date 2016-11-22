using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http.Routing;
using SDesk.Web.WebApi.Regexes;

namespace SDesk.Web.WebApi.Constraints
{
    /// <summary>
    /// 
    /// </summary>
    public class JiraIdConstraint : IHttpRouteConstraint
    {
        private readonly Regex _jiraIdRegex = new JiraRegex().Get();
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
            object value;
            if (values.TryGetValue(parameterName, out value) && value != null)
            {
                var stringValue = value as string;
                return stringValue != null && _jiraIdRegex.IsMatch(stringValue);
            }
            return false;
        }
    }
}