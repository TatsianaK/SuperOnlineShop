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
        public void RecountPriceTest() {
            var controller = new ShoppingCartController(new ShoppingCartRepositoryTest(), new ShoppingCartTest());

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

    public class FakeShoppingCartRepository : IShoppingCartRepository {
        public System.Collections.Generic.List<Models.ShoppingCartItem> GetItems(System.Collections.Generic.Dictionary<int, int> itemCountPerId) {
            return new List<Models.ShoppingCartItem>();
        }
    }
}
