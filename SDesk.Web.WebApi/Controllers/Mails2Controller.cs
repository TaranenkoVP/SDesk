using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using Epam.Sdesk.Model;
using SDesk.DAL.EF;
using SDesk.Web.WebApi.Attributes;
using Swashbuckle.Swagger.Annotations;

namespace SDesk.Web.WebApi.Controllers
{
    /// <summary>
    /// Controller with mail actions version 2
    /// </summary>
    public class Mails2Controller : ApiController
    {
        private readonly IRepository<Attachement> _attachementRepository;
        private readonly IRepository<Mail> _mailRepository;
        private readonly UnitOfWork<SdeskContext> _unit;

        /// <summary>
        /// Constructor
        /// </summary>
        public Mails2Controller()
        {
            _unit = new UnitOfWork<SdeskContext>();
            _mailRepository = _unit.GetRepository<Mail>();
            _attachementRepository = _unit.GetRepository<Attachement>();
        }

        // GET: api/mails
        /// <summary>
        /// Get all mails 
        /// </summary>
        /// <returns></returns>
        [VersionRoute("api/Mails", 2)]
        public IHttpActionResult Get()
        {
            var mails = _mailRepository.GetAll();
            if (mails == null)
            {
                return NotFound();
            }

            return Ok(mails);
        }

        // GET /api/mails/{id}
        /// <summary>
        /// Get mail by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [VersionRoute("api/Mails/{id}", 2)]
        public IHttpActionResult Get(long id)
        {
            var mail = _mailRepository.GetById(id);
            if (mail == null)
            {
                return NotFound();
            }

            return Ok(mail);
        }

        // POST: /api/mails
        /// <summary>
        /// Create new mail
        /// </summary>
        /// <param name="mail"></param>
        /// <returns></returns>
        [VersionRoute("api/Mails", 2)]
        public IHttpActionResult Post([FromBody] Mail mail)
        {
            if (mail == null)
            {
                return NotFound();
            }
            _mailRepository.Add(mail);
            _unit.Save();
            return Created(Request.RequestUri + "/" + mail.Id, mail);
        }

        // PUT: /api/mails/{id}
        /// <summary>
        /// Update mail
        /// </summary>
        /// <param name="id"></param>
        /// <param name="mail"></param>
        /// <returns></returns>
        [VersionRoute("api/Mails/{id}", 2)]
        public IHttpActionResult Put(long id, [FromBody] Mail mail)
        {
            if ((mail == null) || (mail.Id != id))
            {
                return BadRequest();
            }
            IHttpActionResult answerHttpActionResult;
            if (_mailRepository.GetById(id) == null)
            {
                answerHttpActionResult = Created(Request.RequestUri + "/" + mail.Id, mail);
            }
            else
            {
                answerHttpActionResult = Ok(mail);
            }
            _mailRepository.Update(mail);
            _unit.Save();

            return answerHttpActionResult;
        }

        // DELETE: /api/mails/{id}
        /// <summary>
        /// Delete mail by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [VersionRoute("api/Mails/{id}", 2)]
        public IHttpActionResult Delete(long id)
        {
            var mail = _mailRepository.GetById(id);
            if (mail == null)
            {
                return NotFound();
            }
            _mailRepository.Delete(mail);
            _unit.Save();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET api/mails/{id}/attachements
        /// <summary>
        /// Get Attachements by mail Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [VersionRoute("api/mails/{id:int:min(1)}/attachements", 2)]
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
        /// <summary>
        /// Get aatachement by mail id and attachement id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="attId"></param>
        /// <returns></returns>
        [HttpGet]
        [VersionRoute("api/mails/{id:int:min(1)}/attachements/{attId}", 2)]
        public IHttpActionResult AttachementsByMailIdAndAttachementId(int id, int attId)
        {
            var attachment =
                _attachementRepository.GetAll().Where(s => s.MailId == id).FirstOrDefault(t => t.Id == attId);
            if (attachment == null)
            {
                return NotFound();
            }
            return Ok(attachment);
        }

        //GET api/mails/{id}/attachements/{attId}? extention = { ext }
        /// <summary>
        /// Get aatachement by mail id and attachement id with filter by extention
        /// </summary>
        /// <param name="id"></param>
        /// <param name="attId"></param>
        /// <param name="extention"></param>
        /// <returns></returns>
        [HttpGet]
        [VersionRoute("api/mails/{id:int:min(1)}/attachements/{attId}", 2)]
        public IHttpActionResult AttachementsById(int id, int attId, string extention = null)
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

        //GET api/mails/{id}/attachements/{attId}?extention={ext}?status={status}
        /// <summary>
        /// Get aatachement by mail id and attachement id with filters by extention and status
        /// </summary>
        /// <param name="id"></param>
        /// <param name="attId"></param>
        /// <param name="extention"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpGet]
        [VersionRoute("api/mails/{id:int:min(1)}/attachements{attId}", 2)]
        public IHttpActionResult AttachementsById(int id, int attId, string extention = null, int status = 0)
        {
            var attachment =
                _attachementRepository.GetAll().Where(a => a.MailId == id).FirstOrDefault(t => t.Id == attId);
            if (attachment == null)
            {
                return NotFound();
            }
            if (!string.IsNullOrEmpty(extention) && !attachment.FileExtention.Contains(extention))
            {
                return NotFound();
            }
            if ((status != 0) && (attachment.StatusId != status))
            {
                return NotFound();
            }

            return Ok(attachment);
        }

        //POST api/mails/{id}/attachements
        /// <summary>
        /// Create the attachment
        /// </summary>
        /// <param name="id"></param>
        /// <param name="attachment"></param>
        /// <returns></returns>
        [HttpPost]
        [VersionRoute("api/mails/{id:int:min(1)}/attachements", 2)]
        public IHttpActionResult CreateAttachment(int id, [FromBody] Attachement attachment)
        {
            if ((attachment == null) || (id != attachment.MailId))
            {
                return BadRequest();
            }
            var isSuccess = _attachementRepository.Add(attachment);
            _unit.Save();
            if (!isSuccess)
            {
                return BadRequest();
            }
            return Created("/api/mails/" + id + "/attachments/" + attachment.Id, attachment);
        }

        // PUT: api/mails/{id}/attachements/{attId}
        /// <summary>
        /// Update the attachment
        /// </summary>
        /// <param name="id"></param>
        /// <param name="attId"></param>
        /// <param name="attachment"></param>
        /// <returns></returns>
        [HttpPut]
        [VersionRoute("api/mails/{id:int:min(1)}/attachements/{attId}", 2)]
        public IHttpActionResult UpdateAttachment(int id, int attId, [FromBody] Attachement attachment)
        {
            if ((attachment == null) || (attachment.MailId != id) || (attachment.Id != attId))
            {
                return BadRequest();
            }
            var attachementToUpdate = _attachementRepository.GetAll().FirstOrDefault(x => (x.MailId == id) && (x.Id == attId));
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
            var isSuccess = _attachementRepository.Update(attachment);
            _unit.Save();

            return !isSuccess ? InternalServerError() : answerHttpActionResult;
        }

        // DELETE api/mails/{id}/attachements/{attId}
        /// <summary>
        /// Delete the attachment
        /// </summary>
        /// <param name="id"></param>
        /// <param name="attId"></param>
        /// <returns></returns>
        [HttpDelete]
        [VersionRoute("api/mails/{id:int:min(1)}/attachements/{attId}", 2)]
        public IHttpActionResult AttachmentDel(int id, int attId)
        {
            var attachementToDelete =
                _attachementRepository.GetAll().FirstOrDefault(x => (x.MailId == id) && (x.Id == attId));
            if (attachementToDelete == null)
            {
                return BadRequest();
            }
            var isSuccess = _attachementRepository.Delete(attachementToDelete);
            _unit.Save();
            if (!isSuccess)
            {
                return InternalServerError();
            }
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}