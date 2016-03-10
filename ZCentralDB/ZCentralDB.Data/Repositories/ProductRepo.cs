using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCentralDB.Models;
using ZCentralDB.Models.ViewModels;

namespace ZCentralDB.Data.Repositories
{
    public class ProductRepo
    {
        private ApplicationDbContext _db;

        public ProductRepo()
        {
            _db = new ApplicationDbContext();
        }

        public void SaveProductList(List<UploadProductListVM> vm)
        {
            if (vm == null) return;

            Product product = null;
            Detail detail = null;
            Vendor vendor = null;
            Category category = null;

            foreach (UploadProductListVM prod in vm)
            {
                // Vendor Details
                vendor = null;
                vendor = _db.Vendors.Where(v => v.Name == prod.Vendor).FirstOrDefault();
                if (vendor == null)
                {
                    vendor = new Vendor();
                    _db.Vendors.Add(vendor);
                }

                vendor.Name = prod.Vendor;
                _db.SaveChanges();

                // Category Details
                category = null;
                category = _db.Categories.Where(c => c.Name == prod.Category).FirstOrDefault();
                if (category == null)
                {
                    category = new Category();
                    _db.Categories.Add(category);
                }

                category.Name = prod.Category;
                _db.SaveChanges();

                // Create New Product if necessary
                product = null;
                product = _db.Products.Where(p => p.Name == prod.Name && p.ShortDesc == prod.ShortDesc && p.LongDesc == prod.LongDesc).FirstOrDefault();
                if (product == null)
                {
                    product = new Product
                    {
                        Vendor = vendor,
                        Category = category
                    };
                    _db.Products.Add(product);
                }
                else
                {
                    product.Vendor = vendor;
                    product.Category = category;
                }

                product.Name = prod.Name;
                product.ShortDesc = prod.ShortDesc;
                product.LongDesc = prod.LongDesc;
                product.ImageUrl = prod.ImageUrl;
                product.Weight = prod.Weight;
                product.Fabric = prod.Fabric;
                product.Certifications = prod.Certifications;
                if (product.Details == null) product.Details = new List<Detail>();
                _db.SaveChanges();

                // Add Or Update Detail Item
                detail = null;
                detail = _db.Details.Where(d => d.Product.Id == product.Id && d.Sku == prod.Sku && d.Color == prod.Color && d.Size == prod.Size && d.Length == prod.Length && d.Waist == prod.Waist).FirstOrDefault();
                if (detail == null)
                {
                    detail = new Detail
                    {
                        Product = product
                    };
                    _db.Details.Add(detail);
                }
                if (!product.Details.Contains(detail)) product.Details.Add(detail);

                detail.Upc = prod.Upc;
                detail.Sku = prod.Sku;
                detail.Color = prod.Color;
                detail.Size = prod.Size;
                detail.Waist = prod.Waist;
                detail.Length = prod.Length;
                detail.Price = prod.Price;
                detail.OldPrice = prod.OldPrice;
                _db.SaveChanges();
            }
        }
    }
}
