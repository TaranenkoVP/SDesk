using System.Collections.Generic;
using System.Web.Http.Routing;
using SDesk.Web.WebApi.Constraints;

namespace SDesk.Web.WebApi.Attributes
{
    public class VersionRouteAttribute : RouteFactoryAttribute
    {
        public VersionRouteAttribute(string template)
            : base(template)
        {
        }

        public override IDictionary<string, object> Constraints
        {
            get
            {
                var constraints = new HttpRouteValueDictionary();
                constraints.Add("version", new VersionConstraint());
                return constraints;
            }
        }
    }
}