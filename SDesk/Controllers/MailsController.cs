using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Epam.Sdesk.Model;
using SDesk.BLL.Services;
using SDesk.DAL.EF;

namespace SDesk.WebApi.Controllers
{
    public class MailsController : ApiController
    {
        private MailService _mailService;
        public MailsController()
        {
            _mailService = new MailService(new UnitOfWork<SdeskContext>());
            _mailService.Add(new Mail( ) { Subject = "Subject1", Body = "body1", Priority = Priority .High});
        }

        // GET: api/Mails
        public IEnumerable<string> Get()
        {
            var t= _mailService.GetAll();
            return new string[] { "value1", "value2" };
        }

        // GET: api/Mails/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Mails
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Mails/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Mails/5
        public void Delete(int id)
        {
        }
    }
}
