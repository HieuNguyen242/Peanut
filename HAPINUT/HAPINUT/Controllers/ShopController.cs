using HAPINUT.DTO;
using HAPINUT.Models.Shop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HAPINUT.Controllers
{
    public class ShopController : Controller
    {
        // GET: Shop
        [Authorize]
        public ActionResult Index()
        {
            IEnumerable<ProductViewModel> listProduct;
            using (Db db = new Db())
            {
                listProduct = db.Products.ToArray().Select(p => new ProductViewModel(p)).ToList();
            }
            return View(listProduct);
        }

        public ActionResult ProductDetails(int id)
        {
            Product dto;
            ProductViewModel productVM;

            using (Db db = new Db())
            {
                if(!db.Products.Any(x=>x.Id.Equals(id)))
                {
                    return RedirectToAction("Index", "Shop");
                }

                // Init product
                dto = db.Products.Where(x => x.Id == id).FirstOrDefault();

                productVM = new ProductViewModel(dto);
            }


            // Get gallery images
            productVM.GalleryImages = Directory.EnumerateFiles(Server.MapPath("~/Images/Products/" + id + "/Gallery"))
                                                .Select(fn => Path.GetFileName(fn));

            // Return view with model
            return View("ProductDetails", productVM);
        }
    }
}