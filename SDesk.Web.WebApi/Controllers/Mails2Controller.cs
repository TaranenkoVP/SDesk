using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using Epam.Sdesk.Model;
using SDesk.DAL.EF;
using SDesk.Web.WebApi.Attributes;

namespace SDesk.Web.WebApi.Controllers
{
    public class Mails2Controller : ApiController
    {
        private readonly IRepository<Attachement> _attachementRepository;
        private readonly IRepository<Mail> _mailRepository;
        private readonly IUnitOfWork _unit;

        public Mails2Controller()
        {
            _unit = new UnitOfWork<SdeskContext>();
            _mailRepository = _unit.GetRepository<Mail>();
            _attachementRepository = _unit.GetRepository<Attachement>();
        }

        // GET: api/Mails
        [VersionRoute("api/Mails")]
        public IHttpActionResult GetAll()
        {
            var mails = _mailRepository.GetAll();
            if (mails == null)
                return NotFound();

            return Ok(mails);
        }

        // GET /api/mails/{id}
        [VersionRoute("api/Mails/{id}")]
        public IHttpActionResult Get(long id)
        {
            var mail = _mailRepository.GetById(id);
            if (mail == null)
                return NotFound();

            return Ok(mail);
        }

        // POST: /api/mails
        [VersionRoute("api/mails")]
        public IHttpActionResult Post([FromBody] Mail mail)
        {
            if (mail == null)
                return NotFound();

            _mailRepository.Add(mail);
            return Created(Request.RequestUri + "/" + mail.Id, mail);
        }

        // PUT: /api/mails/{id}
        [VersionRoute("api/mails/{id}")]
        public IHttpActionResult Put(long id, [FromBody] Mail mail)
        {
            if ((mail == null) || (mail.Id != id))
                return BadRequest();

            IHttpActionResult answerHttpActionResult;
            if (_mailRepository.GetById(id) == null)
                answerHttpActionResult = Created(Request.RequestUri + "/" + mail.Id, mail);
            else
                answerHttpActionResult = Ok(mail);

            _mailRepository.Update(mail);
            return answerHttpActionResult;
        }

        // DELETE: /api/mails/{id}
        [VersionRoute("api/mails/{id}")]
        public IHttpActionResult Delete(long id)
        {
            var mail = _mailRepository.GetById(id);
            if (mail == null)
                return NotFound();

            _mailRepository.Delete(mail);
            return Ok(id);
        }

        // GET api/mails/{id}/attachements
        [HttpGet]
        [VersionRoute("api/mails/{id:int:min(1)}/attachements")]
        public IHttpActionResult AttachementsByMailIdV2(int id)
        {
            var attachments = _attachementRepository.GetAll().Where(s => s.MailId == id).ToArray();
            if (!attachments.Any())
                return NotFound();

            return Ok(attachments);
        }

        //GET api/mails/{id}/attachements/{attId}
        [HttpGet]
        [VersionRoute("api/mails/{id:int:min(1)}/attachements/{attId}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IHttpActionResult AttachmentById(int id, int attId)
        {
            var attachment = _attachementRepository.GetAll().Where(s => s.MailId == id).FirstOrDefault(t => t.Id == attId);
            if (attachment == null)
                return NotFound();

            return Ok(attachment);
        }

        //GET api/mails/{id}/attachements/{attId}?extention={ext}
        [HttpGet]
        [VersionRoute("api/mails/{id:int:min(1)}/attachements/{attId}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IHttpActionResult AttachementsByIdByExtention(int id, int attId, string extention = null)
        {
            var attachment = _attachementRepository.GetAll().Where(a => a.MailId == id).FirstOrDefault(t => t.Id == attId);
            if (attachment == null)
                return NotFound();
            if (!string.IsNullOrEmpty(extention) && !attachment.FileExtention.Contains(extention))
                return NotFound();

            return Ok(attachment);
        }

        [HttpGet]
        [VersionRoute("api/mails/{id:int:min(1)}/attachements/{attId}")]
        public IHttpActionResult AttachementsByIdByExtentionAndStatus(int id, int attId, string extention = null,
            int status = 0)
        {
            var attachment =
                _attachementRepository.GetAll().Where(a => a.MailId == id).FirstOrDefault(t => t.Id == attId);
            if (attachment == null)
                return NotFound();
            if (!string.IsNullOrEmpty(extention) && !attachment.FileExtention.Contains(extention))
                return NotFound();
            if ((status != 0) && (attachment.StatusId != status))
                return NotFound();

            return Ok(attachment);
        }

        //POST api/mails/{id}/attachements
        [HttpPost]
        [VersionRoute("api/mails/{id:int:min(1)}/attachements")]
        public IHttpActionResult CreateAttachment(int id, [FromBody] Attachement attachment)
        {
            if ((attachment == null) || (id != attachment.MailId))
                return BadRequest();

            var isSuccess = _attachementRepository.Add(attachment);
            if (!isSuccess)
                return BadRequest();

            return Created("/api/mails/" + id + "/attachments/" + attachment.Id, attachment);
        }

        // PUT: api/mails/{id}/attachements/{attId}
        [HttpPut]
        [VersionRoute("api/mails/{id:int:min(1)}/attachements/{attId}")]
        public IHttpActionResult UpdateAttachment(int id, int attId, [FromBody] Attachement attachment)
        {
            if ((attachment == null) || (attachment.MailId != id) || (attachment.Id != attId))
                return BadRequest();

            var attachementToUpdate =
                _attachementRepository.GetAll().FirstOrDefault(x => (x.MailId == id) && (x.Id == attId));
            if (attachementToUpdate == null)
                return BadRequest();

            IHttpActionResult answerHttpActionResult;
            if (_attachementRepository.GetById(id) == null)
                answerHttpActionResult = Created("/api/mails/" + id + "/attachments/" + attachment.Id, attachment);
            else
                answerHttpActionResult = Ok(attachment);

            var isSuccess = _attachementRepository.Update(attachment);
            return !isSuccess ? InternalServerError() : answerHttpActionResult;
        }

        // DELETE api/mails/{id}/attachements/{attId}
        [HttpDelete]
        [VersionRoute("api/mails/{id:int:min(1)}/attachements/{attId}")]
        public IHttpActionResult AttachmentDel(int id, int attId)
        {
            var attachementToDelete =
                _attachementRepository.GetAll().FirstOrDefault(x => (x.MailId == id) && (x.Id == attId));
            if (attachementToDelete == null)
                return BadRequest();

            var isSuccess = _attachementRepository.Delete(attachementToDelete);
            if (!isSuccess)
                return InternalServerError();

            return Ok(attId);
        }
    }
}