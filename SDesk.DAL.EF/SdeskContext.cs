using System.Data.Entity;
using Epam.Sdesk.Model;

namespace SDesk.DAL.EF
{
    public class SdeskContext : DbContext
    {
        public SdeskContext()
            : base("DefaultConnection") 
        {
            Database.SetInitializer(new SdeskContextInitializer());
        } 

        public DbSet<Mail> Mails { get; set; }
        public DbSet<Attachement> Attachements { get; set; }
    }
}
