using System.Web.Http;
using Epam.Sdesk.Model;
using SDesk.DAL.EF;
using SDesk.Web.WebApi.Attributes;

namespace SDesk.Web.WebApi.Controllers
{
    /// <summary>
    /// Controller with mail actions 
    /// </summary>
    public class MailsController : ApiController
    {
        private readonly IRepository<Mail> _mailRepository;
        private readonly UnitOfWork<SdeskContext> _unit;

        /// <summary>
        /// Constructor
        /// </summary>
        public MailsController()
        {
            _unit = new UnitOfWork<SdeskContext>();
            _mailRepository = _unit.GetRepository<Mail>();
        }

        // GET: api/mails
        /// <summary>
        /// Get all mails 
        /// </summary>
        /// <returns></returns>
        [VersionRoute("api/Mails", 1)]
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
        [VersionRoute("api/Mails/{id}", 1)]
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
        [VersionRoute("api/Mails", 1)]
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
        [VersionRoute("api/Mails/{id}", 1)]
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
        [VersionRoute("api/Mails/{id}", 1)]
        public IHttpActionResult Delete(long id)
        {
            var mail = _mailRepository.GetById(id);
            if (mail == null)
            {
                return NotFound();
            }
            _mailRepository.Delete(mail);
            _unit.Save();

            return Ok(id);
        }
    }
}