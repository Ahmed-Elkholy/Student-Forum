using CUFE_SF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CUFE_SF.Controllers
{

    public class PostQuestionController : Controller
    {
        DBManager db = new DBManager();
        
        public ActionResult Post()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PostAQuestion(string question,string title)
        {
            int numOfRows = db.AddQuestion(LoginInfo.LoginUsername, question, LoginInfo.Course,title);
            if (numOfRows == 0) return View("~/Views/PostQuestion/PostFail.cshtml");
            return View("~/Views/PostQuestion/PostSuccess.cshtml");
          
        }
    }
}