using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SuperOnlineShop.Models;
using Umbraco.Web.Mvc;

namespace SuperOnlineShop.Controllers {
    public class CommentsController : SurfaceController {
        //
        // GET: /Comments/

        public ActionResult Index(int? id) {
            List<Comment> result = new List<Comment>();
            if (id.HasValue) {
                List<Comment> comments = new List<Comment>();
                comments.Add(new Comment { ProductId = 1078, UserName = "Tanya", Title = "Very good device", Text = "Cool device! Buy it!", SubmitDate = DateTime.Now, Rating = 4 });
                comments.Add(new Comment { ProductId = 1079, UserName = "Vanya", Title = "Worth to try", Text = "Cool device! Buy it!", SubmitDate = DateTime.Now, Rating = 2 });
                comments.Add(new Comment { ProductId = 1079, UserName = "Nastishka", Title = "Good device", Text = "Cool device!", SubmitDate = DateTime.Now, Rating = 5 });
                comments.Add(new Comment { ProductId = 1078, UserName = "Tanya", Title = "Not bad", Text = "Not bad", SubmitDate = DateTime.Now, Rating = 3 });
            
                result = comments.Where(comment => comment.ProductId == id).ToList();
                return View(result);
            }
            return View();
        }

        public ActionResult AddComment(Comment comment) {
            if(comment.ProductId!=null){
                //add comment
            }
            return RedirectToAction("Index", new {id=comment.ProductId});
        }

    }
}
