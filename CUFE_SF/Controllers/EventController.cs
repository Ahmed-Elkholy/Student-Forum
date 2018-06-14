using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CUFE_SF.Models;

namespace CUFE_SF.Controllers
{
    public class EventController : Controller
    {
        DBManager db = new DBManager();

        public ActionResult Index()
        {
            List<string> list = db.SelectAllEvents();
            ViewBag.list = list;
            ViewBag.count = list.Count();
            return View();
        }

        [HttpGet]
        public ActionResult ViewEvent(string EName)
        {
            List<string> list = db.SelectEvent(EName);
            ViewBag.list = list;
            return View();
        }
    }
}