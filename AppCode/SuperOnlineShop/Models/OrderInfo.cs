using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SuperOnlineShop.Models {
    public class OrderInfo {
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        public string Address { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string AdditionalInfo { get; set; }
        public Dictionary<int, int> orderedProducts { get; set; }
    }
}