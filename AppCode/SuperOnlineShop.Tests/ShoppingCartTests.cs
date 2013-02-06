using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SuperOnlineShop.Controllers;
using SuperOnlineShop.Helpers;

namespace SuperOnlineShop.Tests {
    [TestClass]
    public class ShoppingCartTests {

        IShoppingCart defaultShoppingCart = new ShoppingCartTest();
        ControllerContext controllerContext; 
        Mock<ControllerContext> context;

        [TestInitialize]
        public void SetUp() {
            context = new Mock<ControllerContext>();
            var mockSession = new Mock<HttpSessionStateBase>();
            context.Setup(p => p.HttpContext.Session).Returns(mockSession.Object);
            context.Setup(p => p.HttpContext.Session[ShoppingCart.CartSessionKey]).Returns(new Dictionary<int, int>());
            controllerContext = context.Object;
        }

        [TestMethod]
        public void AddItemsToCart() {
            ShoppingCart cart = new ShoppingCart();

            cart.AddItemsToCart(controllerContext, 1078, 1);
            Dictionary<int, int> items = (Dictionary<int, int>)controllerContext.HttpContext.Session[ShoppingCart.CartSessionKey];
            
            Assert.AreEqual(1,items.Count(), "Count is incorrect");
        }

        [TestMethod]
        public void GetItemsFromCart() {
            ShoppingCart cart = new ShoppingCart();

            Dictionary<int, int> cartItems = new Dictionary<int,int>();
            cartItems.Add(1078,1);
            cartItems.Add(1079,2);

            context.Setup(p => p.HttpContext.Session[ShoppingCart.CartSessionKey]).Returns(cartItems);
            
            Dictionary<int, int> items = cart.GetItems(controllerContext);
            Assert.AreEqual(2,items.Count(), "Count is incorrect");
        }

        [TestMethod]
        public void UpdateItemsInCart() {
            ShoppingCart cart = new ShoppingCart();

            Dictionary<int, int> cartItems = new Dictionary<int,int>();
            cartItems.Add(1078,1);
            cartItems.Add(1079,2);

            context.Setup(p => p.HttpContext.Session[ShoppingCart.CartSessionKey]).Returns(cartItems);

            Dictionary<int, int> updatedCartItems = new Dictionary<int,int>();
            updatedCartItems.Add(1078,2);
            updatedCartItems.Add(1079,3);
            
            cart.UpdateCartItems(controllerContext, updatedCartItems);

            Dictionary<int, int> items = (Dictionary<int, int>)controllerContext.HttpContext.Session[ShoppingCart.CartSessionKey];
            
            Assert.AreEqual(2,items.Count(), "Count is incorrect");
            Assert.AreEqual(2,items[1078], "Item's count is not updated");
            Assert.AreEqual(3,items[1079], "Item's count is not updated");
        }

        [TestMethod]
        public void DeleteItemsFromCart() {
            ShoppingCart cart = new ShoppingCart();

            Dictionary<int, int> cartItems = new Dictionary<int,int>();
            cartItems.Add(1078,1);
            cartItems.Add(1079,2);

            context.Setup(p => p.HttpContext.Session[ShoppingCart.CartSessionKey]).Returns(cartItems);

            
            cart.DeleteItem(controllerContext, 1078);

            Dictionary<int, int> items = (Dictionary<int, int>)controllerContext.HttpContext.Session[ShoppingCart.CartSessionKey];
            
            Assert.AreEqual(1,items.Count(), "Count is incorrect");
            Assert.AreEqual(1079,items.First().Key, "Delete is incorrect");
            Assert.AreEqual(2,items[1079], "Item's count is not updated");

        }

    }
}
