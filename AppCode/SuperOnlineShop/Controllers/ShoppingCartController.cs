using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SuperOnlineShop.Models;
using Umbraco.Web.Mvc;

namespace SuperOnlineShop.Controllers {
    public class ShoppingCartController : SurfaceController {//SurfaceController {
        //
        // GET: /ShoppnigCart/

        public ActionResult Index() {
            List<ShoppingCartItem> shoppingCartItems = GetItems();//for testing
            ViewBag.TotalPrice = shoppingCartItems.Sum(item => item.Price*item.Count);
            return View(shoppingCartItems);
        }

        public ActionResult RecountPrice(){
            return Json(1340); //for testing
        }

        private List<ShoppingCartItem> GetItems(){
            List<ShoppingCartItem> result = new List<ShoppingCartItem>();
            result.Add(new ShoppingCartItem{Id=1, Name = "Nokia Lumia 920", Price=890, Count=1});
            result.Add(new ShoppingCartItem{Id=2, Name = "Nokia Lumia 820", Price=690, Count=2});
            return result;

        }

        [HttpPost]
        public ActionResult AddToCart(int? id, int? count)
        {
            return Json(new { status = "ok" });
        }
    }
}
