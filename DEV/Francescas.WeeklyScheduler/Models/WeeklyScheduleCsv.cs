using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Francescas.WeeklyScheduler.Models
{
    public class WeeklyScheduleCsv
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Boutique")]
        public int? STORE { get; set; }

        [DisplayName("Sale Date")]
        public string SALEDATE { get; set; }

        public double? OVR { get; set; }

        [DisplayName("Task Name")]
        public string TASK { get; set; }

        [DisplayName("Task Hours")]
        public int? HOURS { get; set; }

        public DateTime? SentDateTime { get; set; }
    }
}