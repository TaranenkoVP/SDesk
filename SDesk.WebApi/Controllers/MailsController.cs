using System.Web.Http;
using Epam.Sdesk.Model;
using SDesk.DAL.EF;

namespace SDesk.WebApi.Controllers
{
    public class MailsController : ApiController
    {
        private readonly IRepository<Mail> _mailRepository;
        private readonly IUnitOfWork _unit;

        public MailsController()
        {
            _unit = new UnitOfWork<SdeskContext>();
            _mailRepository = _unit.GetRepository<Mail>();
        }

        // GET: api/Mails
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
        public IHttpActionResult Post([FromBody] Mail mail)
        {
            if (mail == null)
            {
                return NotFound();
            }
            _mailRepository.Add(mail);
            return Created(Request.RequestUri + "/" + mail.Id, mail);
        }

        // PUT: /api/mails/{id}
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
            return answerHttpActionResult;
        }

        // DELETE: /api/mails/{id}
        public IHttpActionResult Delete(long id)
        {
            var mail = _mailRepository.GetById(id);
            if (mail == null)
            {
                return NotFound();
            }
            _mailRepository.Delete(mail);
            return Ok(id);
        }
    }
}