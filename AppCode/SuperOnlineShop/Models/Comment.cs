using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuperOnlineShop.Models {
    public class Comment {
        public int Id { get; set;}
        public int ProductId { get; set;}
        public string UserName {get; set;}
        public string Title {get; set; }
        public string Text { get; set;}
        public DateTime SubmitDate {get; set;}
    }
}