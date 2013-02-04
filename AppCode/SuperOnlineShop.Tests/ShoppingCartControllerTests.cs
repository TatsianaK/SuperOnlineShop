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
        [TestMethod]
        public void ShouldIndexActionReturnDefaultView()
        {
            var umbracoContext = FormatterServices.GetUninitializedObject(typeof(UmbracoContext)) as UmbracoContext;
            var routingContextProperty = typeof(UmbracoContext).GetProperty("RoutingContext", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var routingContext = FormatterServices.GetUninitializedObject(typeof(RoutingContext)) as RoutingContext;
            routingContextProperty.SetValue(umbracoContext, routingContext);
            var currentProperty = typeof(UmbracoContext).GetProperty("Current", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            currentProperty.SetValue(null, umbracoContext);

            var controller = new ShoppingCartController(new FakeShoppingCartRepository(), new FakeShoppingCart());

            var actionResult = controller.Index();

            Assert.IsInstanceOfType(actionResult, typeof(ViewResult), "Index action returns not a ViewResult");
        }
    }

    public class FakeShoppingCart : IShoppingCart
    {
        private Dictionary<int, int> cart = new System.Collections.Generic.Dictionary<int, int>();

        public Dictionary<int, int> GetItems(ControllerContext context)
        {
            return cart;
            //throw new NotImplementedException();
        }

        public void DeleteItem(ControllerContext context, int id)
        {
            cart.Remove(id);
            //throw new NotImplementedException();
        }

        public void UpdateCartItems(ControllerContext context, System.Collections.Generic.Dictionary<int, int> items)
        {
            cart = items;
            //throw new NotImplementedException();
        }

        public void AddItemsToCart(ControllerContext context, int id, int count)
        {
            if (!cart.ContainsKey(id))
            {
                cart[id] = 0;
            }

            cart[id] = cart[id] + count;
            //throw new NotImplementedException();
        }
    }

    public class FakeShoppingCartRepository : IShoppingCartRepository
    {
        public System.Collections.Generic.List<Models.ShoppingCartItem> GetItems(System.Collections.Generic.Dictionary<int, int> itemCountPerId)
        {
            //throw new NotImplementedException();
            return new List<Models.ShoppingCartItem>();
        }
    }
}
