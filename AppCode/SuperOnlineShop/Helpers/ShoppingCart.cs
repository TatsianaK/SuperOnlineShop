using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SuperOnlineShop.Controllers;

namespace SuperOnlineShop.Helpers {
    public class ShoppingCart : IShoppingCart {

        //private ControllerContext controllerContext;
        private const string CartSessionKey = "ShoppingCartItems";

        //public ShoppingCart(ControllerContext context){
        //    controllerContext = context;
        //}

        public Dictionary<int, int> GetItems(ControllerContext controllerContext) {
            Dictionary<int, int> sessionShoppingCartItems = new Dictionary<int, int>();
            if (controllerContext.HttpContext.Session[CartSessionKey] != null) {
                sessionShoppingCartItems = (Dictionary<int, int>)controllerContext.HttpContext.Session[CartSessionKey];
            }
            return sessionShoppingCartItems;
        }

        public void DeleteItem(ControllerContext controllerContext, int id) {
             if (controllerContext.HttpContext.Session[CartSessionKey] != null) {
                Dictionary<int, int> shoppingCartItems = (Dictionary<int, int>)controllerContext.HttpContext.Session[CartSessionKey];
                if (shoppingCartItems.Any(item => item.Key == id)) {
                    shoppingCartItems.Remove(id);
                }
            }
        }

        public void UpdateCartItems(ControllerContext controllerContext, Dictionary<int, int> items) {
            Dictionary<int, int> cartItems = GetItems(controllerContext);
            foreach (var item in items) {
                cartItems[item.Key] = item.Value;
            }
        }

        public void AddItemsToCart(ControllerContext controllerContext, int id, int count) {
            Dictionary<int, int> shoppingCartItems = new Dictionary<int, int>();

            if (controllerContext.HttpContext.Session[CartSessionKey] != null) {
                shoppingCartItems = (Dictionary<int, int>)controllerContext.HttpContext.Session[CartSessionKey];
            }

            if (shoppingCartItems.Any(item => item.Key == id)) {
                shoppingCartItems[id] += count;
            } else {
                shoppingCartItems[id] = count;
            }

            controllerContext.HttpContext.Session[CartSessionKey] = shoppingCartItems;
        }
    }
}