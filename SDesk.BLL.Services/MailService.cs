using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Epam.Sdesk.Model;
using SDesk.DAL.EF;

namespace SDesk.BLL.Services
{
    public class MailService
    {
        private IRepository<Mail> _repo;
        private IUnitOfWork _unit;

        public MailService(IUnitOfWork unit)
        {
            _repo = unit.GetRepository<Mail>();
            _unit = unit;
        }

        public void Add(Mail mail)
        {
            if (mail == null)
            {
                throw new ArgumentNullException(nameof(mail));
            }
            _repo.Add(mail);
            _unit.Save();
        }

        public void Edit(Mail mail)
        {
            if (mail == null)
            {
                throw new ArgumentNullException(nameof(mail));
            }
            _repo.Update(mail);
            _unit.Save();
        }

        public void Delete(long id)
        {
            var mail = _repo.GetById(id);
            if (mail == null)
            {
                throw new ArgumentNullException(nameof(id));
            }
            _repo.Delete(mail);
            _unit.Save();
        }

        public IEnumerable<Mail> GetAll()
        {
            return _repo.GetAll();
        }

        public Mail GetById(long id)
        {
            return _repo.GetById(id);
        }

    }
}
