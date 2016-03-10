using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZCentralDB.Data.Repositories
{
    public class ProductRepo
    {
        private ApplicationDbContext _db;

        public ProductRepo()
        {
            _db = new ApplicationDbContext();
        }

        public bool IsAdmin(string id)
        {
            ApplicationUser user = _db.Users.Where(u => u.Id == id).FirstOrDefault();

            return (user.Type.IsAdmin && !user.Type.IsRetired);
        }
    }
}
