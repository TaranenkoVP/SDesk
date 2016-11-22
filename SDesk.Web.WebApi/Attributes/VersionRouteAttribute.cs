using System.Collections.Generic;
using System.Web.Http.Routing;
using System.Web.Services.Description;
using SDesk.Web.WebApi.Constraints;

namespace SDesk.Web.WebApi.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    public class VersionRouteAttribute : RouteFactoryAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="template"></param>
        /// <param name="allowedVersion"></param>
        public VersionRouteAttribute(string template, int allowedVersion)
            : base(template)
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

        /// <inheritdoc />
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