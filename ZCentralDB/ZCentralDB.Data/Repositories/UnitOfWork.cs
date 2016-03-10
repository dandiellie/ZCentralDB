using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZCentralDB.Data.Repositories
{
    public class UnitOfWork
    {
        public ProductRepo Product { get; set; }

        public UnitOfWork()
        {
            Product = new ProductRepo();
        }
    }
}
