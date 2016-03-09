using Mvc.Mailer;

namespace Francescas.WeeklyScheduler.Mailers
{ 
    public interface IUserMailer
    {
			MvcMailMessage Welcome(int id);
			MvcMailMessage PasswordReset();
	}
}