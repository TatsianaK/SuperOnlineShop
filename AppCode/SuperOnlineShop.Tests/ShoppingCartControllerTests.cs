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

namespace SuperOnlineShop.Tests
{
    [TestClass]
    public class ShoppingCartControllerTests
    {
        [TestInitialize]
        public void SetUp()
        {
            var umbracoContext = FormatterServices.GetUninitializedObject(typeof(UmbracoContext)) as UmbracoContext;
            var routingContextProperty = typeof(UmbracoContext).GetProperty("RoutingContext", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var routingContext = FormatterServices.GetUninitializedObject(typeof(RoutingContext)) as RoutingContext;
            routingContextProperty.SetValue(umbracoContext, routingContext);
            var currentProperty = typeof(UmbracoContext).GetProperty("Current", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            currentProperty.SetValue(null, umbracoContext);
        }

        [TestMethod]
        public void IndexActionShouldReturnDefaultView()
        {
            var controller = new ShoppingCartController(new FakeShoppingCartRepository(), new ShoppingCartTest());

            var actionResult = controller.Index();

            Assert.IsInstanceOfType(actionResult, typeof(ViewResult), "Index action returns not a ViewResult");
        }

        [TestMethod]
        public void LoginActionShouldReturnDefaultView()
        {
            var controller = new ShoppingCartController(new FakeShoppingCartRepository(), new ShoppingCartTest());

            var actionResult = controller.Login();

            Assert.IsInstanceOfType(actionResult, typeof(ViewResult), "Login action returns not a ViewResult");
        }

        [TestMethod]
        public void RegisterActionShouldReturnDefaultView()
        {
            var controller = new ShoppingCartController(new FakeShoppingCartRepository(), new ShoppingCartTest());

            var actionResult = controller.Register();

            Assert.IsInstanceOfType(actionResult, typeof(ViewResult), "Register action returns not a ViewResult");
        }

        [TestMethod]
        public void SuccessfullyRegisteredActionShouldReturnSuccessfullyRegisteredView()
        {
            var controller = new ShoppingCartController(new FakeShoppingCartRepository(), new ShoppingCartTest());

            var actionResult = controller.SuccessfullyRegistered();

            Assert.IsInstanceOfType(actionResult, typeof(ViewResult), "SuccessfullyRegistered action returns not a ViewResult");
            Assert.AreEqual("SuccessfullyRegistered", (actionResult as ViewResult).ViewName, "SuccessfullyRegistered action does not return SuccessfullyRegistered view");
        }
    }

    public class FakeShoppingCartRepository : IShoppingCartRepository
    {
        public System.Collections.Generic.List<Models.ShoppingCartItem> GetItems(System.Collections.Generic.Dictionary<int, int> itemCountPerId)
        {
            return new List<Models.ShoppingCartItem>();
        }
    }
}
