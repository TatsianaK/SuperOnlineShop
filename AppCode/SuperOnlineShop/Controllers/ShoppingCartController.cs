using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SuperOnlineShop.Helpers;
using SuperOnlineShop.Models;
using Umbraco.Web.Mvc;

namespace SuperOnlineShop.Controllers {
    public class ShoppingCartController : SurfaceController {
        //
        // GET: /ShoppnigCart/

        public ActionResult Index() {
            var connectionString = "server=epbyminw1853\\sqlexpress;database=Umbraco;user id=UmbracoUser;password=q123456789";
            IEnumerable<int> ids = GetShoppingCartItemIds();

            List<ShoppingCartItem> shoppingCartItems = ShoppingCartHelper.GetItems(connectionString, ids);

            ViewBag.TotalPrice = shoppingCartItems.Sum(item => item.Price*item.Count);

            return View(shoppingCartItems);
        }

        public ActionResult RecountPrice(){
            return Json(1340); //for testing
        }

        private List<int> GetShoppingCartItemIds(){
            List<int> ids = new List<int>(){1078,1079};//for testing
            return ids;

        }

        [HttpPost]
        public ActionResult AddToCart(int? id, int? count)
        {
            return Json(new { status = "ok" });
        }
    }
}
