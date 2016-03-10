using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ZCentralDB.Data.Repositories;
using ZCentralDB.Models.ViewModels;

namespace ZCentralDB.Web.Controllers.api
{
    public class ProductsController : ApiController
    {
        private UnitOfWork _unit;

        public ProductsController()
        {
            _unit = new UnitOfWork();
        }

        [ActionName("upload")]
        public IHttpActionResult PostUpload(List<UploadProductListVM> vm)
        {
            _unit.Product.SaveProductList(vm);

            return Ok();
        }
    }
}
