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
using Umbraco.Web;
using System.Web.Routing;

namespace SuperOnlineShop.Controllers {
    public class ShoppingCartController : SurfaceController {
        //
        // GET: /ShoppnigCart/
        private IShoppingCartRepository repository;
        private IShoppingCart cart;

        public ShoppingCartController(){
            repository = new ShoppingCartRepository();
            cart = new ShoppingCart();
        }
        
        public ShoppingCartController(IShoppingCartRepository shoppingCartRepository, IShoppingCart shoppingCart){
            repository = shoppingCartRepository;
            cart = shoppingCart;
        }

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
            Dictionary<int,int> updatedItems = model.Select(item => new { item.Id, item.Count }).ToDictionary(item => item.Id, item => item.Count);
            cart.UpdateCartItems(this.ControllerContext, updatedItems);

            List<ShoppingCartItem> shoppingCartItems =  GetShoppingCartItems();
            return View(shoppingCartItems);
        }

        public ActionResult Delete(int id) {
            cart.DeleteItem(this.ControllerContext, id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AddToCart(int id, int count) {
            try {
                cart.AddItemsToCart(this.ControllerContext, id,count);
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
            ViewBag.Message = "Thank you. Your order will be processed. We will contact you within 10 minutes";
            // test code
            var OrderedProducts = (Dictionary<int, int>)Session["ShoppingCartItems"];
            
            if (Session["ShoppingCartItems"] != null)
            {
                UpdateBoughtProductsCount(OrderedProducts);
            }
            orderInfo.orderedProducts = OrderedProducts;
            //send orderInfo to admin
            //UpdateBoughtProductsCount(orderInfo.orderedProducts);
            return View();
        }

        public JsonResult GetCartSummary(){
            List<ShoppingCartItem> shoppingCartItems = GetShoppingCartItems();

            return Json(new {Count = shoppingCartItems.Sum(item=> item.Count), TotalPrice = shoppingCartItems.Sum(item=> item.Count*item.Price)});
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
            newMember.Password = registerModel.Password;
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

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            //var m = Member.GetMemberFromLoginName(loginModel.Login);
            var m = Member.GetMemberFromLoginNameAndPassword(loginModel.Login, loginModel.Password);
            if (m == null)
            {
                ModelState.AddModelError("login", "Login and password do not match!!!");

                return View();
            }

            ControllerContext.HttpContext.Response.Redirect("/");

            return Content("Logged in!!!");
        }

        private List<ShoppingCartItem> GetShoppingCartItems() {

            Dictionary<int, int> cartItems = cart.GetItems(this.ControllerContext);
            List<ShoppingCartItem> shoppingCartItems = repository.GetItems(cartItems);
            return shoppingCartItems;
        }

        //private void AddItemsToSession(int id, int count) {
        //    Dictionary<int, int> shoppingCartItems = new Dictionary<int, int>();

        //    if (Session["ShoppingCartItems"] != null) {
        //        shoppingCartItems = (Dictionary<int, int>)Session["ShoppingCartItems"];
        //    }

        //    if (shoppingCartItems.Any(item => item.Key == id)) {
        //        shoppingCartItems[id] += count;
        //    } else {
        //        shoppingCartItems[id] = count;
        //    }

        //    Session["ShoppingCartItems"] = shoppingCartItems;
        //}

        //private void UpdateCartItems(Dictionary<int, int> items) {
        //    Dictionary<int, int> cartItems = GetItemsFromSession();
        //    foreach (var item in items) {
        //        cartItems[item.Key] = item.Value;
        //    }
        //}

        //private Dictionary<int, int> GetItemsFromSession() {
        //    Dictionary<int, int> sessionShoppingCartItems = new Dictionary<int, int>();
        //    if (Session["ShoppingCartItems"] != null) {
        //        sessionShoppingCartItems = (Dictionary<int, int>)Session["ShoppingCartItems"];
        //    }
        //    return sessionShoppingCartItems;
        //}

        //private void DeleteItemFromSession(int id) {
        //    if (Session["ShoppingCartItems"] != null) {
        //        Dictionary<int, int> shoppingCartItems = (Dictionary<int, int>)Session["ShoppingCartItems"];
        //        if (shoppingCartItems.Any(item => item.Key == id)) {
        //            shoppingCartItems.Remove(id);
        //        }
        //    }
        //}

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
