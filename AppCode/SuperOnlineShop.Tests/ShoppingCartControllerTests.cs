using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuperOnlineShop.Controllers;
using System.Web.Mvc;
using System.Web.Routing;
using Umbraco.Web;
using Umbraco.Web.Routing;
using System.Runtime.Serialization;
using System.Reflection;
using SuperOnlineShop.Helpers;
using System.Collections.Generic;
using SuperOnlineShop.Models;
using System.Linq;

namespace SuperOnlineShop.Tests {
    [TestClass]
    public class ShoppingCartControllerTests {
        [TestInitialize]
        public void SetUp() {
            var umbracoContext = FormatterServices.GetUninitializedObject(typeof(UmbracoContext)) as UmbracoContext;
            var routingContextProperty = typeof(UmbracoContext).GetProperty("RoutingContext", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var routingContext = FormatterServices.GetUninitializedObject(typeof(RoutingContext)) as RoutingContext;
            routingContextProperty.SetValue(umbracoContext, routingContext);
            var currentProperty = typeof(UmbracoContext).GetProperty("Current", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            currentProperty.SetValue(null, umbracoContext);
        }

        [TestMethod]
        public void IndexActionShouldReturnDefaultView() {
            var controller = new ShoppingCartController(new FakeShoppingCartRepository(), new ShoppingCartTest());

            var actionResult = controller.Index();

            Assert.IsInstanceOfType(actionResult, typeof(ViewResult), "Index action returns not a ViewResult");
        }

        [TestMethod]
        public void LoginActionShouldReturnDefaultView() {
            var controller = new ShoppingCartController(new FakeShoppingCartRepository(), new ShoppingCartTest());

            var actionResult = controller.Login();

            Assert.IsInstanceOfType(actionResult, typeof(ViewResult), "Login action returns not a ViewResult");
        }

        [TestMethod]
        public void RegisterActionShouldReturnDefaultView() {
            var controller = new ShoppingCartController(new FakeShoppingCartRepository(), new ShoppingCartTest());

            var actionResult = controller.Register();

            Assert.IsInstanceOfType(actionResult, typeof(ViewResult), "Register action returns not a ViewResult");
        }

        [TestMethod]
        public void RegisterPostActionShouldReturnSuccessfullyRegisteredView() {
            var controller = new ShoppingCartController(new FakeShoppingCartRepository(), new ShoppingCartTest());

            var registerModel = new RegisterModel {
                Name = "user01",
                Email = "user01@domain.com",
                Password = "pass01"
            };

            var actionResult = controller.Register(registerModel);

            Assert.AreEqual("SuccessfullyRegistered", (actionResult as ViewResult).ViewName, "Registered post action should return SuccessfullyRegistered view");
        }

        [TestMethod]
        public void SuccessfullyRegisteredActionShouldReturnDefaultView() {
            var controller = new ShoppingCartController(new FakeShoppingCartRepository(), new ShoppingCartTest());

            var actionResult = controller.SuccessfullyRegistered();

            Assert.IsInstanceOfType(actionResult, typeof(ViewResult), "SuccessfullyRegistered action returns not a ViewResult");
        }

        [TestMethod]
        public void GetCartSummaryActionShouldReturnJsonResult() {
            var controller = new ShoppingCartController(new FakeShoppingCartRepository(), new ShoppingCartTest());

            var actionResult = controller.GetCartSummary();

            Assert.IsInstanceOfType(actionResult, typeof(JsonResult), "GetCartSummary action returns not a JsonResult");
        }

        [TestMethod]
        public void IndexItemsNumber() {
            var shoppingCart = new ShoppingCartTest();
            shoppingCart.AddItemsToCart(null, 1078, 1);
            shoppingCart.AddItemsToCart(null, 1079, 2);
            var controller = new ShoppingCartController(new ShoppingCartRepositoryTest(), shoppingCart);

            var actionResult = controller.Index() as ViewResult;
            var model = ModelFromActionResult<IEnumerable<ShoppingCartItem>>(actionResult);
            
            Assert.AreEqual(model.Count(), 2, "Number of items is not correct");
        }

        [TestMethod]
        public void IndexDisplayCorrectItem() {
            var shoppingCart = new ShoppingCartTest();
            shoppingCart.AddItemsToCart(null, 1078, 1);
            shoppingCart.AddItemsToCart(null, 1079, 2);
            var controller = new ShoppingCartController(new ShoppingCartRepositoryTest(), shoppingCart);

            var actionResult = controller.Index() as ViewResult;
            var model = ModelFromActionResult<IEnumerable<ShoppingCartItem>>(actionResult);
            
            Assert.AreEqual(model.First().Id, 1078, "Id is not correct");
            Assert.AreEqual(model.Last().Id, 1079, "Id is not correct");
        }    
            


        [TestMethod]
        public void RecountPriceForOneItem() {
            var shoppingCart = new ShoppingCartTest();
            shoppingCart.AddItemsToCart(null, 1078, 1);
            shoppingCart.AddItemsToCart(null, 1079, 2);
            var controller = new ShoppingCartController(new ShoppingCartRepositoryTest(), shoppingCart);

            var actionResult = (ViewResult)controller.Index();
            IEnumerable<ShoppingCartItem> model = new List<ShoppingCartItem>(new ShoppingCartItem[]{
                new ShoppingCartItem{Id=1078, Count =2}
            });

            actionResult = (ViewResult)controller.Index(model);
            var recountedSum = ModelFromActionResult<IEnumerable<ShoppingCartItem>>(actionResult).Sum(item => item.Price * item.Count);

            Assert.AreEqual(recountedSum, 600, "Recount is incorrect");
        }

        [TestMethod]
        public void RecountPriceForTwoItems() {
            var shoppingCart = new ShoppingCartTest();
            shoppingCart.AddItemsToCart(null, 1078, 1);
            shoppingCart.AddItemsToCart(null, 1079, 2);
            var controller = new ShoppingCartController(new ShoppingCartRepositoryTest(), shoppingCart);

            var actionResult = (ViewResult)controller.Index();
            IEnumerable<ShoppingCartItem> model = new List<ShoppingCartItem>(new ShoppingCartItem[]{
                new ShoppingCartItem{Id=1078, Count =2},
                new ShoppingCartItem{Id=1079, Count =3}
            });

            actionResult = (ViewResult)controller.Index(model);
            var recountedSum = ModelFromActionResult<IEnumerable<ShoppingCartItem>>(actionResult).Sum(item => item.Price * item.Count);

            Assert.AreEqual(recountedSum, 800, "Recount is incorrect");
        }

        [TestMethod]
        public void DeleteActionRedirectToIndex() {
            var shoppingCart = new ShoppingCartTest();
            shoppingCart.AddItemsToCart(null, 1078, 1);
            shoppingCart.AddItemsToCart(null, 1079, 2);
            var controller = new ShoppingCartController(new ShoppingCartRepositoryTest(), shoppingCart);

            var redirectResult = controller.Delete(1078) as RedirectToRouteResult;

            Assert.IsNotNull(redirectResult, "Redirect result is null");
            Assert.AreEqual(redirectResult.RouteValues["action"], "Index", "Delete action should be redirected to Index");
        }

        [TestMethod]
        public void SuccessffullyDeleteItem() {
            var shoppingCart = new ShoppingCartTest();
            shoppingCart.AddItemsToCart(null, 1078, 1);
            shoppingCart.AddItemsToCart(null, 1079, 2);

            var controller = new ShoppingCartController(new ShoppingCartRepositoryTest(), shoppingCart);

            var redirectResult = controller.Delete(1078) as RedirectToRouteResult;
            var actionResult = controller.Index();

            var model = ModelFromActionResult<IEnumerable<ShoppingCartItem>>(actionResult);
            Assert.AreEqual(model.Count(), 1, "Delete action should delete one item");
        }

        public T ModelFromActionResult<T>(ActionResult actionResult) {
            object model;
            if (actionResult.GetType() == typeof(ViewResult)) {
                ViewResult viewResult = (ViewResult)actionResult;
                model = viewResult.Model;
            } else if (actionResult.GetType() == typeof(PartialViewResult)) {
                PartialViewResult partialViewResult = (PartialViewResult)actionResult;
                model = partialViewResult.Model;
            } else {
                throw new InvalidOperationException(string.Format("Actionresult of type {0} is not supported by ModelFromResult extractor.", actionResult.GetType()));
            }
            T typedModel = (T)model;
            return typedModel;
        }
    }

    public class FakeShoppingCartRepository : IShoppingCartRepository {
        public System.Collections.Generic.List<Models.ShoppingCartItem> GetItems(System.Collections.Generic.Dictionary<int, int> itemCountPerId) {
            return new List<Models.ShoppingCartItem>();
        }
    }
}
