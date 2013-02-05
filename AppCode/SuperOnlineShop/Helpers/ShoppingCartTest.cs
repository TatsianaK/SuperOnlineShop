using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SuperOnlineShop.Helpers {
    public class ShoppingCartTest : IShoppingCart {

        private static Dictionary<int, int> shoppingCartItems;

        public ShoppingCartTest(){
            if(shoppingCartItems==null){
                shoppingCartItems = new Dictionary<int,int>();
                shoppingCartItems.Add(1078,1);
                shoppingCartItems.Add(1079,2);
            }
        }

        public Dictionary<int, int> GetItems(ControllerContext controllerContext){
            return shoppingCartItems;
        }
        public void DeleteItem(ControllerContext controllerContext, int id){
            if (shoppingCartItems.Any(item => item.Key == id)) {
                shoppingCartItems.Remove(id);
            }
        }

        public void UpdateCartItems(ControllerContext controllerContext, Dictionary<int, int> items){
            foreach (var item in items) {
                shoppingCartItems[item.Key] = item.Value;
            }
        }

        public void AddItemsToCart(ControllerContext controllerContext, int id, int count){
            if (shoppingCartItems.Any(item => item.Key == id)) {
                shoppingCartItems[id] += count;
            } else {
                shoppingCartItems[id] = count;
            }
        }
    }
}