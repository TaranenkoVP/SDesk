using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SDesk.WebApi.Controllers
{
    [RoutePrefix("api/mails/{id}/attachements")]
    public class AttachementsController : ApiController
    {
        [HttpGet]
        [Route]
        public IEnumerable<string> AttachementsByMailId(int id)
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet]
        [Route]
        public string AttachementsByExtention(int id, string extention)
        {
            return "value1";
        }

        [HttpGet]
        [Route]
        public string AttachementsByExtentionAndStatus(int id, string extention = null, int status = 0)
        {
            return "value2";
        }

        // POST: api/Attachements
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Attachements/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Attachements/5
        public void Delete(int id)
        {
        }
    }
}
