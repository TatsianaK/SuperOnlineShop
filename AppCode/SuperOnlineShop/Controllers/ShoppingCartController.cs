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
using umbraco.cms.businesslogic.member;

namespace SuperOnlineShop.Controllers {
    public class ShoppingCartController : SurfaceController {
        //
        // GET: /ShoppnigCart/

        public ActionResult Index() {
            List<ShoppingCartItem> shoppingCartItems = GetShoppingCartItems();
            return View(shoppingCartItems);
        }

        /// <summary>
        /// Recount price method
        /// </summary>
        /// <param name="model">Shopping cart items</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(IEnumerable<ShoppingCartItem> model) {
            var updatedItems = model.Select(item => new { item.Id, item.Count }).ToDictionary(item => item.Id, item => item.Count);
            UpdateCartItems(updatedItems);

            List<ShoppingCartItem> shoppingCartItems = GetShoppingCartItems();
            return View(shoppingCartItems);
        }

        public ActionResult Delete(int id) {
            DeleteItemFromSession(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AddToCart(int id, int count) {
            try {
                AddItemsToSession(id, count);
                return Json(new { status = "ok" });
            } catch (Exception ex) {
                return Json(new { error = ex.Message });
            }
        }

        public ActionResult Order() {
            return View();
        }

        [HttpPost]
        public ActionResult Order(OrderInfo orderInfo) {
            ViewBag.Message = "Thank you. Your order will be processed";
            // test code
            if (Session["ShoppingCartItems"] != null)
            {
                UpdateBoughtProductsCount((Dictionary<int, int>)Session["ShoppingCartItems"]);
            }
            //UpdateBoughtProductsCount(orderInfo.orderedProducts);
            return View();
        }

        public JsonResult GetCartSummary(){
            List<ShoppingCartItem> shoppingCartItems = GetShoppingCartItems();

            return Json(new {Count = shoppingCartItems.Count, TotalPrice = shoppingCartItems.Sum(item=> item.Count*item.Price)});
        }


        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterModel registerModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (Member.GetMemberFromEmail(registerModel.Email) != null)
            {
                ModelState.AddModelError("email", "There is already a user with such an email!!!");
                return View();
            }
            if (Member.GetMemberByName(registerModel.Name, false).Count() > 0)
            {
                ModelState.AddModelError("name", "There is already a user with such a name!!!");
                return View();
            }

            MemberType demoMemberType = MemberType.GetByAlias("Customer");
            Member newMember = Member.MakeNew(registerModel.Name, demoMemberType, new umbraco.BusinessLogic.User(0));

            newMember.Email = registerModel.Email;
            newMember.Password = "1";
            newMember.LoginName = registerModel.Name;

            newMember.getProperty("address").Value = registerModel.Address; //set value of property with alias ‘address’
            newMember.getProperty("phoneNumber").Value = registerModel.PhoneNumber; //set value of property with alias ‘phoneNumber’
            newMember.getProperty("fullName").Value = registerModel.FullName; //set value of property with alias ‘fullName’

            newMember.Save();

            return View("SuccessfullyRegistered");
        }

        public ActionResult SuccessfullyRegistered()
        {
            return View();
        }


        private List<ShoppingCartItem> GetShoppingCartItems() {
            var connectionString = ConfigurationManager.AppSettings["umbracoDbDSN"];

            Dictionary<int, int> sessionShoppingCartItems = GetItemsFromSession();

            List<ShoppingCartItem> shoppingCartItems = ShoppingCartHelper.GetItems(connectionString, sessionShoppingCartItems);
            return shoppingCartItems;
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

        private void UpdateCartItems(Dictionary<int, int> items) {
            Dictionary<int, int> cartItems = GetItemsFromSession();
            foreach (var item in items) {
                cartItems[item.Key] = item.Value;
            }
        }

        private Dictionary<int, int> GetItemsFromSession() {
            Dictionary<int, int> sessionShoppingCartItems = new Dictionary<int, int>();
            if (Session["ShoppingCartItems"] != null) {
                sessionShoppingCartItems = (Dictionary<int, int>)Session["ShoppingCartItems"];
            }
            return sessionShoppingCartItems;
        }

        private void DeleteItemFromSession(int id) {
            if (Session["ShoppingCartItems"] != null) {
                Dictionary<int, int> shoppingCartItems = (Dictionary<int, int>)Session["ShoppingCartItems"];
                if (shoppingCartItems.Any(item => item.Key == id)) {
                    shoppingCartItems.Remove(id);
                }
            }
        }

        private void UpdateBoughtProductsCount(Dictionary<int, int> orderedProducts) {
            using (DbProviderDataContext dbContext = new DbProviderDataContext(ConfigurationManager.AppSettings["umbracoDbDSN"])) {
                if (orderedProducts != null) {
                    foreach (var kvp in orderedProducts) {
                        var boughtProduct = dbContext.BoughtProducts.Where(x => x.NodeId == kvp.Key).FirstOrDefault();
                        if (boughtProduct == null) {
                            dbContext.BoughtProducts.InsertOnSubmit(new BoughtProduct { NodeId = kvp.Key, Count = kvp.Value });
                        } else {
                            boughtProduct.Count = boughtProduct.Count + kvp.Value;
                        }
                    }

                    dbContext.SubmitChanges();
                }
            }
        }
    }
}
