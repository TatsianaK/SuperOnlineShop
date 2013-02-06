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

        [TestMethod]
        public void MyTest() {
            ShoppingCart cart = new ShoppingCart();

            var context = new Mock<ControllerContext>();
            var mockSession = new Mock<HttpSessionStateBase>();
            context.Setup(p => p.HttpContext.Session).Returns(mockSession.Object);

            //context.Object.HttpContext.Session["test"]=123;

            cart.AddItemsToCart(context.Object, 1078, 1);
            Dictionary<int, int> items = cart.GetItems(context.Object);
            Assert.AreEqual(1,items.Count(), "Count is incorrect");
        }

    }
}
