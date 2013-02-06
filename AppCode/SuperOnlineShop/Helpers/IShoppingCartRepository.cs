using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SuperOnlineShop.Models;

namespace SuperOnlineShop.Helpers {
    public interface IShoppingCartRepository {
        List<ShoppingCartItem> GetItems(Dictionary<int, int> itemCountPerId);
    }
}