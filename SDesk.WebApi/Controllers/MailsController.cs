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
        }

        // GET: api/Mails
        public IHttpActionResult Get()
        {
            var mails = _mailService.GetAll();
            if (mails == null)
            {
                return NotFound();
            }
            return Ok(mails);
        }

        // GET: api/Mails/5
        public IHttpActionResult Get(long id)
        {
            var mail = _mailService.GetById(id);
            if (mail == null)
            {
                return NotFound();
            }
            return Ok(mail);
        }

        // POST: api/Mails
        public IHttpActionResult Post([FromBody] Mail mail)
        {
            if (mail == null)
            {
                return NotFound();
            }
            try
            {
                _mailService.Add(mail);
            }
            catch (Exception)
            {
                return InternalServerError();
            }
            return Created(Request.RequestUri + "/" + mail.Id, mail);
        }

        // PUT: api/Mails/5
        public IHttpActionResult Put(long id, [FromBody]Mail mail)
        {
            if (mail == null || mail.Id != id)
            {
                return BadRequest();
            }
            IHttpActionResult answerHttpActionResult;
            if (_mailService.GetById(id) == null)
            {
                answerHttpActionResult = Created(Request.RequestUri + "/" + mail.Id, mail);
            }
            else
            {
                answerHttpActionResult = Ok(mail);
            }
            try
            {
                _mailService.Edit(mail);
            }
            catch (Exception)
            {
                return InternalServerError();
            }
            return answerHttpActionResult;
        }

        // DELETE: api/Mails/5
        public IHttpActionResult Delete(long id)
        {
            var mail = _mailService.GetById(id);
            if (mail == null)
            {
                return NotFound();
            }
            try
            {
                _mailService.Delete(id);
            }
            catch (Exception)
            {
                return InternalServerError();
            }
            return Ok(id);
        }
    }
}
