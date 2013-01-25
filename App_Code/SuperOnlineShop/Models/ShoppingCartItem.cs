using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuperOnlineShop.Models {
    public class ShoppingCartItem {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public int Price { get; set; }
    }
}