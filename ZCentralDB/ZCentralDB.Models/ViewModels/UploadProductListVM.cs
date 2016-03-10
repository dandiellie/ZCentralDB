using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZCentralDB.Models.ViewModels
{
    public class UploadProductListVM
    {
        public string Name { get; set; }
        public string ShortDesc { get; set; }
        public string LongDesc { get; set; }
        public string ImageUrl { get; set; }
        public decimal? Weight { get; set; }
        public string Fabric { get; set; }
        public string Certifications { get; set; }

        public string Upc { get; set; }
        public string Sku { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public string Waist { get; set; }
        public string Length { get; set; }
        public decimal? Price { get; set; }
        public decimal? OldPrice { get; set; }

        public string Vendor { get; set; }
        public string Category { get; set; }
    }
}
