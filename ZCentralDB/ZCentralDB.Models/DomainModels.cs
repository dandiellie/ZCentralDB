using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZCentralDB.Models
{
    /// <summary>
    /// All other Domain Models inherit from the Universe.
    /// </summary>
    public class Universe
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Deleted { get; set; }
    }

    /// <summary>
    /// Vendor represents all the companies that Z|Companies uses as vendors.
    /// </summary>
    public class Vendor : Universe
    {
        public string WebUrl { get; set; }
        public string NopUrl { get; set; }
        public string Desc { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }

    /// <summary>
    /// Organization represents Z|Companies' clients.
    /// </summary>
    public class Organization : Universe
    {
        public ICollection<string> LogoUrls { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }

    /// <summary>
    /// Products are items from NonAPI Vendors and items attached to NopCommerce sites.
    /// </summary>
    public class Product : Universe
    {
        public string ShortDesc { get; set; }
        public string LongDesc { get; set; }
        public string ImageUrl { get; set; }
        public decimal Weight { get; set; }
        public string Fabric { get; set; }
        public string Certifications { get; set; }
        public virtual ICollection<Detail> Details { get; set; }

        [ForeignKey("Vendor")]
        public int VendorId { get; set; }
        public virtual Vendor Vendor { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        [ForeignKey("Organization")]
        public int OrganizationId { get; set; }
        public virtual Organization Organization { get; set; }
    }

    /// <summary>
    /// Category is the NopCommerce Category for the Client's storefront.
    /// </summary>
    public class Category : Universe
    {
        public virtual ICollection<Product> Products { get; set; }
    }

    /// <summary>
    /// Detail distinguishes the NopCommerce attributes of a product such as color, size, and SKU.
    /// </summary>
    public class Detail : Universe
    {
        public string Upc { get; set; }
        public string Sku { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public string Waist { get; set; }
        public string Length { get; set; }
        public decimal Price { get; set; }
        public decimal OldPrice { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
