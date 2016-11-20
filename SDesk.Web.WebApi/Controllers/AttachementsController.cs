﻿using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using Epam.Sdesk.Model;
using SDesk.DAL.EF;
using Swashbuckle.Swagger.Annotations;

namespace SDesk.Web.WebApi.Controllers
{
    [RoutePrefix("api/mails/{id:int:min(1)}/attachements")]
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
        [Route("", Name = "AttachementsByMailId")]
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
        [Route("{attId}", Name = "AttachementsByMailIdAndAttachementId")]
        //[SwaggerOperation("get")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IHttpActionResult AttachementsByMailIdAndAttachementId(int id, int attId)
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
        [Route("{attId}", Name = "AttachementsByMailIdAndAttachementIdAndExtention")]
        [ApiExplorerSettings(IgnoreApi = true)]
        //[SwaggerOperation("get1",OperationId = "1")]
        public IHttpActionResult AttachementsByIdByExAttachementsByMailIdAndAttachementIdAndExtentiontention(int id, int attId, string extention = null)
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
        [Route("{attId}", Name = "AttachementsByIdByExAttachementsByMailIdAndAttachementIdAndExtentiontentionAndStatus")]
        //[SwaggerOperation("get3")]
        public IHttpActionResult AttachementsByIdByExAttachementsByMailIdAndAttachementIdAndExtentiontentionAndStatus(int id, int attId, string extention = null, int status = 0)
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
            if (attachment == null || id != attachment.MailId)
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

        // PUT: api/mails/{id}/attachements/{attId}
        [HttpPut]
        [Route("{attId}")]
        public IHttpActionResult UpdateAttachment(int id, int attId, [FromBody]Attachement attachment)
        {
            if ((attachment == null) || (attachment.MailId != id) || (attachment.Id != attId))
            {
                return BadRequest();
            }
            var attachementToUpdate = _attachementRepository.GetAll().FirstOrDefault(x => x.MailId == id && x.Id == attId);
            if (attachementToUpdate == null)
            {
                return BadRequest();
            }
            IHttpActionResult answerHttpActionResult;
            if (_attachementRepository.GetById(id) == null)
            {
                answerHttpActionResult = Created("/api/mails/" + id + "/attachments/" + attachment.Id, attachment);
            }
            else
            {
                answerHttpActionResult = Ok(attachment);
            }
            bool isSuccess = _attachementRepository.Update(attachment);
            return !isSuccess ? InternalServerError() : answerHttpActionResult;
        }

        // DELETE api/mails/{id}/attachements/{attId}
        [HttpDelete]
        [Route("{attId}")]
        public IHttpActionResult AttachmentDel(int id, int attId)
        {
            var attachementToDelete = _attachementRepository.GetAll().FirstOrDefault(x => x.MailId == id && x.Id == attId);
            if (attachementToDelete == null)
            {
                return BadRequest();
            }
            bool isSuccess = _attachementRepository.Delete(attachementToDelete);
            if (!isSuccess)
            {
                return InternalServerError();
            }
            return Ok(attId);
        }
    }
}
