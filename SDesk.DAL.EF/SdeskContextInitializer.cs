using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Epam.Sdesk.Model;

namespace SDesk.DAL.EF
{
    public class SdeskContextInitializer : DropCreateDatabaseIfModelChanges<SdeskContext>
    {
        protected override void Seed(SdeskContext context)
        {
            Mail[] mails =
            {
                new Mail() {Subject = "Subject1", Body = "body1", Priority = Priority.High},
                new Mail() {Subject = "Subject2", Body = "body2", Priority = Priority.Low}
            };
            context.Mails.AddRange(mails);

            Attachement[] attachements =
            {
                new Attachement() {FileName = "FileName1", FileExtention = "FileExtention1", StatusId = 1},
                new Attachement() {FileName = "FileName2", FileExtention = "FileExtention2", StatusId = 2}
            };
            context.Attachements.AddRange(attachements);
            context.SaveChanges();
        }
    }
}
//Received = DateTime.Now, Saved = DateTime.Now