using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Francescas.WeeklyScheduler.Models
{
    public class ScheduleMailerSend
    {
        public int StoreNumber { get; set; }
        public string FilePath { get; set; }
        public string StoreEmail { get; set; }
        public byte[] FileBytes { get; set; }
    }
}
