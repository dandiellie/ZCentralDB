using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Francescas.WeeklyScheduler.Models;
using Mvc.Mailer;

namespace Francescas.WeeklyScheduler.Mailers
{
    public interface IScheduleMailer
    {
        MvcMailMessage Schedule(ScheduleMailerSend model);
        MvcMailMessage AnotherSchedule(ScheduleMailerSend model);
    }
}
