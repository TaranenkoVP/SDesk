using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace SDesk.Web.WebApi
{
    public class CustomControllerSelector : DefaultHttpControllerSelector
    {

        private HttpConfiguration _config;
        public CustomControllerSelector(HttpConfiguration config)
            : base(config)
        {
            _config = config;
        }

        public override HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            try
            {
                var controllers = GetControllerMapping();
                var routeData = request.GetRouteData();

                var controllerName = routeData.Values["controller"].ToString();
                HttpControllerDescriptor controllerDescriptor;

                //string versionNum = GetVersionFromQueryString(request);
                 string versionNum = GetVersionFromHeader(request);
                //string versionNum = GetVersionFromAcceptHeader(request);



                if (versionNum.Equals("V1"))
                {
                    if (controllers.TryGetValue(controllerName, out controllerDescriptor))
                    {
                        return controllerDescriptor;
                    }
                }
                else
                {
                    controllerName = string.Concat(controllerName, "V2");
                    if (controllers.TryGetValue(controllerName, out controllerDescriptor))
                    {
                        return controllerDescriptor;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to Get Query String Values from URL to get the version number
        /// </summary>
        /// <param name="request">HttpRequestMessage: Current Request made through Browser or Fiddler</param>
        /// <returns>Version Number</returns>
        private string GetVersionFromQueryString(HttpRequestMessage request)
        {
            var versionStr = HttpUtility.ParseQueryString(request.RequestUri.Query);

            if (versionStr[0] != null)
            {
                return versionStr[0];
            }
            return "V1";
        }
        /// <summary>
        /// Method to Get Header Values.
        /// </summary>
        /// <param name="request">HttpRequestMessage: Current Request made through Browser or Fiddler</param>
        /// <returns>Version Number</returns>
        private string GetVersionFromHeader(HttpRequestMessage request)
        {
            const string HEADER_NAME = "api-version";

            if (request.Headers.Contains(HEADER_NAME))
            {
                var versionHeader = request.Headers.GetValues(HEADER_NAME).FirstOrDefault();
                if (versionHeader != null)
                {
                    return versionHeader;
                }
            }

            return "V1";
        }

        /// <summary>
        /// Method to Get Accept Header Values.
        /// </summary>
        /// <param name="request">HttpRequestMessage: Current Request made through Browser or Fiddler</param>
        /// <returns>Version Number</returns>
        private string GetVersionFromAcceptHeader(HttpRequestMessage request)
        {
            var acceptHeader = request.Headers.Accept;

            foreach (var mime in acceptHeader)
            {
                if (mime.MediaType == "application/json")
                {
                    return "V2";
                }
                else if (mime.MediaType == "application/xml")
                {
                    return "V1";
                }
                else { return "V1"; }

            }
            return "V1";
        }
    }
}