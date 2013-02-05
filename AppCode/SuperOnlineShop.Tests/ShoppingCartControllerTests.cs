using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuperOnlineShop.Controllers;
using System.Web.Mvc;
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
        IShoppingCart defaultShoppingCart = new ShoppingCartTest();

        [TestInitialize]
        public void SetUp() {
            var umbracoContext = FormatterServices.GetUninitializedObject(typeof(UmbracoContext)) as UmbracoContext;
            var routingContextProperty = typeof(UmbracoContext).GetProperty("RoutingContext", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var routingContext = FormatterServices.GetUninitializedObject(typeof(RoutingContext)) as RoutingContext;
            routingContextProperty.SetValue(umbracoContext, routingContext);
            var currentProperty = typeof(UmbracoContext).GetProperty("Current", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            currentProperty.SetValue(null, umbracoContext);

            defaultShoppingCart = new ShoppingCartTest();
            defaultShoppingCart.AddItemsToCart(null, 1078, 1);
            defaultShoppingCart.AddItemsToCart(null, 1079, 2);
        }

        [TestMethod]
        public void IndexActionShouldReturnDefaultView() {
            var controller = new ShoppingCartController(new ShoppingCartRepositoryTest(), new ShoppingCartTest());

            var actionResult = controller.Index();

            Assert.IsInstanceOfType(actionResult, typeof(ViewResult), "Index action returns not a ViewResult");
        }

        [TestMethod]
        public void LoginActionShouldReturnDefaultView()
        {
            var controller = new ShoppingCartController(new ShoppingCartRepositoryTest(), new ShoppingCartTest());

            var actionResult = controller.Login();

            Assert.IsInstanceOfType(actionResult, typeof(ViewResult), "Login action returns not a ViewResult");
        }

        [TestMethod]
        public void RegisterActionShouldReturnDefaultView()
        {
            var controller = new ShoppingCartController(new ShoppingCartRepositoryTest(), new ShoppingCartTest());

            var actionResult = controller.Register();

            Assert.IsInstanceOfType(actionResult, typeof(ViewResult), "Register action returns not a ViewResult");
        }

        [TestMethod]
        public void RegisterPostActionShouldReturnSuccessfullyRegisteredView()
        {
            var controller = new ShoppingCartController(new ShoppingCartRepositoryTest(), new ShoppingCartTest());

            var registerModel = new RegisterModel
            {
                Name = "user01",
                Email = "user01@domain.com",
                Password = "pass01"
            };

            var actionResult = controller.Register(registerModel);

            Assert.AreEqual("SuccessfullyRegistered", (actionResult as ViewResult).ViewName, "Registered post action should return SuccessfullyRegistered view");
        }

        [TestMethod]
        public void SuccessfullyRegisteredActionShouldReturnDefaultView()
        {
            var controller = new ShoppingCartController(new ShoppingCartRepositoryTest(), new ShoppingCartTest());

            var actionResult = controller.SuccessfullyRegistered();

            Assert.IsInstanceOfType(actionResult, typeof(ViewResult), "SuccessfullyRegistered action returns not a ViewResult");
        }

        [TestMethod]
        public void GetCartSummaryActionShouldReturnJsonResult()
        {
            var controller = new ShoppingCartController(new ShoppingCartRepositoryTest(), new ShoppingCartTest());

            var actionResult = controller.GetCartSummary();

            Assert.IsInstanceOfType(actionResult, typeof(JsonResult), "GetCartSummary action returns not a JsonResult");
        }

        [TestMethod]
        public void GetCartSummaryActionShouldReturnRightCountAndTotalPrice()
        {
            var controller = new ShoppingCartController(new ShoppingCartRepositoryTest(), defaultShoppingCart);

            var actionResult = controller.GetCartSummary();

            var result = (actionResult as JsonResult).Data;

            var countPropertyInfo = result.GetType().GetProperty("Count");
            var countValue = (int)countPropertyInfo.GetValue(result);

            var totalPricePropertyInfo = result.GetType().GetProperty("TotalPrice");
            var totalPriceValue = (int)totalPricePropertyInfo.GetValue(result);

            Assert.AreEqual(2, countValue, "Count is not equal to 2");
            Assert.AreEqual(500, totalPriceValue, "TotalPrice is not equal to 500");
        }

        [TestMethod]
        public void RecountPriceTest() {
            var controller = new ShoppingCartController(new ShoppingCartRepositoryTest(), defaultShoppingCart);

            var actionResult = (ViewResult)controller.Index();
            IEnumerable<ShoppingCartItem> model = new List<ShoppingCartItem>(new ShoppingCartItem[]{
                new ShoppingCartItem{Id=1078, Count =2}
            });

            actionResult = (ViewResult)controller.Index(model);
            var recountedSum = ModelFromActionResult<IEnumerable<ShoppingCartItem>>(actionResult).Sum(item => item.Price * item.Count);

            Assert.AreEqual(recountedSum, 514, "Recount is incorrect");
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
}
