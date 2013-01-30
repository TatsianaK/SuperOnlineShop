using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SuperOnlineShop.Models {
    public class OrderInfo {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [Display(Name = "Enter your name")]
        public string UserName { get; set; }
        [Display(Name = "Enter your address")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Phone is required")]
        [Display(Name = "Enter your phone number")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Enter your email")]
        public string Email { get; set; }
        [Display(Name = "Additional information")]
        public string AdditionalInfo { get; set; }
        public Dictionary<int, int> orderedProducts { get; set; }
        public bool Delivery { get; set; }
    }
}