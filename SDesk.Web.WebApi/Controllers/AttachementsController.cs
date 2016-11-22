using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using Epam.Sdesk.Model;
using SDesk.DAL.EF;
using SDesk.Web.WebApi.Attributes;

namespace SDesk.Web.WebApi.Controllers
{
    /// <summary>
    /// Controller with attachement actions 
    /// </summary>
    public class AttachementsController : ApiController
    {
        private readonly IRepository<Attachement> _attachementRepository;
        private readonly UnitOfWork<SdeskContext> _unit;

        /// <summary>
        /// Constructor
        /// </summary>
        public AttachementsController()
        {
            _unit = new UnitOfWork<SdeskContext>();
            _attachementRepository = _unit.GetRepository<Attachement>();
        }

        // GET api/mails/{id}/attachements
        /// <summary>
        /// Get Attachements by mail Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [VersionRoute("api/mails/{id:int:min(1)}/attachements", 1)]
        public IHttpActionResult AttachementsByMailId(int id)
        {
            var attachments = _attachementRepository.GetAll().Where(s => s.MailId == id).ToArray();
            if (!attachments.Any())
            {
                return NotFound();
            }
            return Ok(attachments);
        }

        //GET api/mails/{id}/attachements/{attId
        /// <summary>
        /// Get aatachement by mail id and attachement id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="attId"></param>
        /// <returns></returns>
        [HttpGet]
        [VersionRoute("api/mails/{id:int:min(1)}/attachements/{attId}", 1)]
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
        [VersionRoute("api/mails/{id:int:min(1)}/attachements/{attId}", 1)]
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
        [VersionRoute("api/mails/{id:int:min(1)}/attachements{attId}", 1)]
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
        [VersionRoute("api/mails/{id:int:min(1)}/attachements", 1)]
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
        [VersionRoute("api/mails/{id:int:min(1)}/attachements/{attId}", 1)]
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
        [VersionRoute("api/mails/{id:int:min(1)}/attachements/{attId}", 1)]
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
            return Ok(attId);
        }
    }
}
