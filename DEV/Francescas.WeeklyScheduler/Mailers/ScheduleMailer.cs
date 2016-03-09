using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using Francescas.WeeklyScheduler.Models;
using Mvc.Mailer;

namespace Francescas.WeeklyScheduler.Mailers
{
    public class ScheduleMailer : MailerBase, IScheduleMailer
    {
        public ScheduleMailer()
		{
			MasterName="_Layout";
		}

        public MvcMailMessage Schedule(ScheduleMailerSend model)
        {
            while (IsFileInUse(model.FilePath))
            {
                Console.WriteLine("File is in use " + model.StoreNumber);
            };

            return Populate(x =>
            {
                x.Subject = "Weekly Schedule for Store " + model.StoreNumber;
                x.ViewName = "Schedule";
                //x.To.Add("jacob.cunningham.sparkhound@gmail.com");
                //x.Attachments.Add(new Attachment(System.Web.HttpContext.Current.Server.MapPath("/ScheduleResources/import.csv")));
                x.To.Add(model.StoreEmail);
                x.Attachments.Add(new Attachment(model.FilePath));
            });
        }

        public MvcMailMessage AnotherSchedule(ScheduleMailerSend model)
        {
            //byte[] fileBytes = System.IO.File.ReadAllBytes(model.FilePath);
            string fileName = model.StoreNumber.ToString() + ".xlsx";
            //return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

                return Populate(x =>
                {
                    x.Subject = "Weekly Schedule for Store " + model.StoreNumber;
                    x.ViewName = "Schedule";
                    //x.To.Add("jacob.cunningham.sparkhound@gmail.com");
                    //x.Attachments.Add(new Attachment(System.Web.HttpContext.Current.Server.MapPath("/ScheduleResources/import.csv")));
                    x.To.Add(model.StoreEmail);
                    x.Attachments.Add(new Attachment(new MemoryStream(model.FileBytes), fileName, System.Net.Mime.MediaTypeNames.Application.Octet));
                });
        }

        public bool IsFileInUse(string path)
        {
            try
            {
                using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read)) { }
            }
            catch (IOException)
            {
                return true;
            }

            return false;
        }
    }
}
