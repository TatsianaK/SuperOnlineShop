using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SuperOnlineShop.Models;

namespace SuperOnlineShop.Helpers {
    public class ShoppingCartRepositoryTest : IShoppingCartRepository{

        private List<ShoppingCartItem> items = new List<ShoppingCartItem>(new  ShoppingCartItem[]{
            new ShoppingCartItem{Id=1078, Name="ItemTest1", Price=123},
            new ShoppingCartItem{Id=1079, Name="ItemTest2", Price=134},
            new ShoppingCartItem{Id=1080, Name="ItemTest3", Price=345}
        });

        public List<ShoppingCartItem> GetItems(Dictionary<int, int> itemCountPerId) {
            List<ShoppingCartItem> result = new List<ShoppingCartItem>();
            result = items.Where(item => itemCountPerId.Keys.Contains(item.Id)).ToList();
            foreach(var item in result){
                item.Count = itemCountPerId[item.Id];
                item.Sum = item.Price * item.Count;
            }
            return result;
        }

    }
}