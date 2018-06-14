using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CUFE_SF.Models;

namespace CUFE_SF.Controllers
{
    public class SAController : Controller
    {
        DBManager db = new DBManager();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateEvent()
        {
            return View();
        }

        public ActionResult UpdateEvent()
        {
            List<string> list = db.SelectEvents(LoginInfo.LoginUsername);
            ViewBag.list = list;
            return View();
        }

        public ActionResult DeleteEvent()
        {
            List<string> list = db.SelectEvents(LoginInfo.LoginUsername);
            ViewBag.list = list; //code list
            return View(); //change this
        }

        [HttpGet]
        public ActionResult AddEvent(string Ename, string Date, string Fee, string Description, string Location)
        {
            int numOfRows = db.AddEvent(LoginInfo.LoginUsername, Ename, Date, Fee, Description, Location);
            if (numOfRows == 0) return View("~/Views/SA/EAFail.cshtml");
            return View("~/Views/SA/EASuccess.cshtml");
        }

        [HttpGet]
        public ActionResult ModifyEvent(string Ename, string Date, string Fee, string Description, string Location)
        {
            db.UpdateEvent(LoginInfo.LoginUsername, Ename, Date, Fee, Description, Location);
            List<string> list = db.SelectEvents(LoginInfo.LoginUsername);
            ViewBag.list = list;
            return View("~/Views/SA/EUSuccess.cshtml");
        }

        [HttpGet]
        public ActionResult RemoveEvent(string EName)
        {
            db.RemoveEvent(LoginInfo.LoginUsername, EName);
            List<string> list = db.SelectEvents(LoginInfo.LoginUsername);
            ViewBag.list = list;
            return View("~/Views/SA/DeleteEvent.cshtml");
        }
    }
}