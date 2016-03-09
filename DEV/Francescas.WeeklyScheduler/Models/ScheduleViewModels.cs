using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Francescas.WeeklyScheduler.Models
{
    public class EditViewModel
    {
        public int Store { get; set; }
        //public DateTime SaleDate { get; set; }
        //public double Ovr { get; set; }
        public DateTime Date { get; set; }
        public IEnumerable<EditRecord> Records{ get; set; }
    }

    public class EditRecord
    {
        [DisplayName("Sale Date")]
        [DisplayFormat(DataFormatString = "{0:dddd, MMM dd, yyyy}")]
        public DateTime SaleDate { get; set; }
        [DisplayName("OVR")]
        public double Ovr { get; set; }
    }

    public class DownloadScheduleViewModel
    {
        public int Store { get; set; }
        public DateTime Date { get; set; }
    }

    public class EmailScheduleViewModel
    {
        public int Store { get; set; }
        public DateTime Date { get; set; }
        [DisplayName("Custom Email")]
        [EmailAddress(ErrorMessage = "Custom Email Address is invalid.")]
        public string CustomEmail { get; set; }
    }

    public class EmailAllSchedulesViewModel
    {
        public DateTime Date { get; set; }
        public List<ManageStore> Stores { get; set; }
        [DisplayName("Custom Email")]
        [EmailAddress(ErrorMessage = "Custom Email Address is invalid.")]
        public string CustomEmail { get; set; }
    }

    public class EditScheduleViewModel
    {
        public int Store { get; set; }
        public DateTime Date { get; set; }
    }

    public class ManageStoresViewModel
    {
        public List<ManageStore> Stores { get; set; }
    }

    public class ManageStore
    {
        public int StoreNumber { get; set; }
        [DisplayName("Last Sent E-Mail")]
        public DateTime? SentDateTime { get; set; }
        [DisplayName("Select All")]
        public bool IsChecked { get; set; }
    }
}
