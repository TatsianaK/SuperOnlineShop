﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuperOnlineShop.Models {
    public class Comment {
        public int Id { get; set;}
        public int UserId {get; set;}
        public string Text { get; set;}
    }
}