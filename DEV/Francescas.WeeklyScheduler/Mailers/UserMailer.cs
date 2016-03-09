using System.Web.UI.WebControls;
using Mvc.Mailer;

namespace Francescas.WeeklyScheduler.Mailers
{ 
    public class UserMailer : MailerBase, IUserMailer 	
	{
		public UserMailer()
		{
			MasterName="_Layout";
		}
		
		public virtual MvcMailMessage Welcome(int id)
		{
			//ViewBag.Data = someObject;
			return Populate(x =>
			{
				x.Subject = "SENT";
				x.ViewName = "Welcome";
				x.To.Add("jacob.cunningham.sparkhound@gmail.com");
                
			});
		}
 
		public virtual MvcMailMessage PasswordReset()
		{
			//ViewBag.Data = someObject;
			return Populate(x =>
			{
				x.Subject = "PasswordReset";
				x.ViewName = "PasswordReset";
				x.To.Add("some-email@example.com");
			});
		}
 	}
}