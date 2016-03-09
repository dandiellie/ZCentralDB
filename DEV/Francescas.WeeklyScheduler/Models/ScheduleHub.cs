using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace Francescas.WeeklyScheduler.Models
{
    public class ScheduleHub : Hub
    {
        public static void MassEmailStatus(int currentBoutique, string message, int totalBoutiques, int boutiquesProcessed)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<ScheduleHub>();
            hubContext.Clients.All.massEmailStatus(currentBoutique, message, totalBoutiques, boutiquesProcessed);
        }
    }
}