using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Epam.Sdesk.Model;
using SDesk.DAL.EF;

namespace SDesk.BLL.Services
{
    public class AttachementService
    {
        private IRepository<Attachement> _repo;
        private IUnitOfWork _unit;

        public AttachementService(IUnitOfWork unit)
        {
            _repo = unit.GetRepository<Attachement>();
            _unit = unit;
        }

        public void Add(Attachement mail)
        {
            if (mail == null)
            {
                throw new ArgumentNullException(nameof(mail));
            }
            _repo.Add(mail);
            _unit.Save();
        }

        public void Edit(Attachement mail)
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

        public IEnumerable<Attachement> GetAllByMailId(int id)
        {
            return _repo.GetAll().Where(s => s.MailId == id);
        }

        public Attachement GetById(long id)
        {
            return _repo.GetById(id);
        }

    }
}
