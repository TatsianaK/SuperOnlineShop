using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SuperOnlineShop.Helpers {
    public interface IShoppingCart {
        Dictionary<int, int> GetItems(ControllerContext context);
        void DeleteItem(ControllerContext context, int id);
        void UpdateCartItems(ControllerContext context, Dictionary<int, int> items);
        void AddItemsToCart(ControllerContext context, int id, int count);
    }
}
