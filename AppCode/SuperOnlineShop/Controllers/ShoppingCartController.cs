using System;
using System.Collections.Generic;
using System.Configuration;
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
            var connectionString = ConfigurationManager.AppSettings["umbracoDbDSN"];

            Dictionary<int, int> sessionShoppingCartItems = GetItemsFromSession();

            List<ShoppingCartItem> shoppingCartItems = ShoppingCartHelper.GetItems(connectionString, sessionShoppingCartItems);

            ViewBag.TotalPrice = shoppingCartItems.Sum(item => item.Price * item.Count);

            return View(shoppingCartItems);
        }

        public ActionResult RecountPrice() {
            return Json(1340); //for testing
        }

        [HttpPost]
        public ActionResult AddToCart(int id, int count) {
            try{
                AddItemsToSession(id, count);
                return Json(new { status = "ok" });
            } catch (Exception ex){
                return Json (new { error = ex.Message });
            }
        }

        private void AddItemsToSession(int id, int count) {
            Dictionary<int, int> shoppingCartItems = new Dictionary<int, int>();

            if (Session["ShoppingCartItems"] != null) {
                shoppingCartItems = (Dictionary<int, int>)Session["ShoppingCartItems"];
            }

            if (shoppingCartItems.Any(item => item.Key == id)) {
                shoppingCartItems[id] += count;
            } else {
                shoppingCartItems[id] = count;
            }

            Session["ShoppingCartItems"] = shoppingCartItems;
        }

        private Dictionary<int, int> GetItemsFromSession(){
            Dictionary<int, int> sessionShoppingCartItems = new Dictionary<int, int>();
            if (Session["ShoppingCartItems"] != null) {
                sessionShoppingCartItems = (Dictionary<int, int>)Session["ShoppingCartItems"];
            }
            return sessionShoppingCartItems;
        }
    }
}
