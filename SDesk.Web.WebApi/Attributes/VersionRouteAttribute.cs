using System.Collections.Generic;
using System.Web.Http.Routing;
using System.Web.Services.Description;
using SDesk.Web.WebApi.Constraints;

namespace SDesk.Web.WebApi.Attributes
{
    public class VersionRouteAttribute : RouteFactoryAttribute
    {
        public VersionRouteAttribute(string template, int allowedVersion, string name)
            : base(template)
        {
            AllowedVersion = allowedVersion;
            Name = name;
        }
        public int AllowedVersion
        {
            get;
            private set;
        }
        public override IDictionary<string, object> Constraints
        {
            get
            {
                var constraints = new HttpRouteValueDictionary();
                constraints.Add("version", new VersionConstraint(AllowedVersion));
                return constraints;
            }
        }
    }
}