using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http.Routing;
using SDesk.WebApi.Regexes;

namespace SDesk.WebApi.Constraints
{
    public class JiraIdConstraint
    {
        private readonly Regex _jiraIdRegex = new JiraRegex().Get();

        public bool Match(HttpRequestMessage request, IHttpRoute route, string parameterName, IDictionary<string, object> values, HttpRouteDirection routeDirection)
        {
            object value;
            if (values.TryGetValue(parameterName, out value) && value != null)
            {
                var stringValue = value as string;
                if (stringValue == null)
                    return false;

                return _jiraIdRegex.IsMatch(stringValue);
            }
            return false;
        }
    }
}