using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web.Http;
using Epam.Sdesk.Model;
using SDesk.DAL.EF;

namespace SDesk.WebApi.Controllers
{
    [RoutePrefix("api/mails/{id}/attachements")]
    public class AttachementsController : ApiController
    {
        private readonly IRepository<Attachement> _attachementRepository;
        private readonly IUnitOfWork _unit;

        public AttachementsController()
        {
            _unit = new UnitOfWork<SdeskContext>();
            _attachementRepository = _unit.GetRepository<Attachement>();
        }
        
        // GET api/mails/{id}/attachements
        [HttpGet]
        public IHttpActionResult AttachementsByMailId(int id)
        {
            var attachments = _attachementRepository.GetAll().Where(s => s.MailId == id).ToArray();
            if (!attachments.Any())
            {
                return NotFound();
            }
            return Ok(attachments);
        }

        //GET api/mails/{id}/attachements/{attId}
        [HttpGet]
        [Route("{attId}")]
        public IHttpActionResult AttachmentById(int id, int attId)
        {
            var attachment = _attachementRepository.GetAll().Where(s => s.MailId == id).FirstOrDefault(t => t.Id == attId);
            if (attachment == null)
            {
                return NotFound();
            }
            return Ok(attachment);
        }

        //GET api/mails/{id}/attachements/{attId}?extention={ext}
        [HttpGet]
        [Route("{attId}")]
        public IHttpActionResult AttachementsByIdByExtention(int id, int attId, string extention = null)
        {
            var attachment = _attachementRepository.GetAll().Where(a => a.MailId == id).FirstOrDefault(t => t.Id == attId);
            if (attachment == null)
            {
                return NotFound();
            }
            if (!string.IsNullOrEmpty(extention) && !attachment.FileExtention.Contains(extention))
            {
                return NotFound();
            }
            return Ok(attachment);
        }

        [HttpGet]
        [Route("{attId}")]
        public IHttpActionResult AttachementsByIdByExtentionAndStatus(int id, int attId, string extention = null, int status = 0)
        {
            var attachment = _attachementRepository.GetAll().Where(a => a.MailId == id).FirstOrDefault(t => t.Id == attId);
            if (attachment == null)
            {
                return NotFound();
            }
            if (!string.IsNullOrEmpty(extention) && !attachment.FileExtention.Contains(extention))
            {
                return NotFound();
            }
            if (status != 0 && attachment.StatusId != status)
            {
                return NotFound();
            }
            return Ok(attachment);
        }

        //POST api/mails/{id}/attachements
        [HttpPost]
        public IHttpActionResult CreateAttachment(int id, [FromBody]Attachement attachment)
        {
            if (attachment == null)
            {
                return BadRequest();
            }
            bool isSuccess = _attachementRepository.Add(attachment);
            if (!isSuccess)
            {
                return BadRequest();
            }
            return Created("/api/mails/" + id + "/attachments/" + attachment.Id, attachment);
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
