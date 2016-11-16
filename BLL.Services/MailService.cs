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

        public MailService(IUnitOfWork unit)
        {
            _repo = unit.GetRepository<Mail>();
        }

        public void Add(Mail mail)
        {
            _repo.Add(mail);
        }

        public IEnumerable<Mail> GetAll()
        {
            return _repo.GetAll();
        }
    }
}
