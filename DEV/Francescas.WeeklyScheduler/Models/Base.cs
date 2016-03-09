using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Francescas.WeeklyScheduler.Models
{
    public class Base
    {
        public Base()
        {
            ModifiedDateTime = DateTime.Now;
            CreatedDateTime = DateTime.Now;
        }

        public int Id { get; set; }
        public DateTime ModifiedDateTime { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}
