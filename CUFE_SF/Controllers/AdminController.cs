using CUFE_SF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;

namespace CUFE_SF.Views.Admin
{
    public class AdminController : Controller
    {
        DBManager db = new DBManager();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewRequests()
        {
            return View();
        }

        public ActionResult ViewSRRequests()
        {
            List<string> list = db.SelectSRRequests();
            ViewBag.list = list;
            ViewBag.count = list.Count();
            return View();
        }

        [HttpPost]
        public ActionResult AcceptSRRequest(string Username)
        {
            int numOfRows = db.RespondRequest('A',Username);
            List<string> list = db.SelectSRRequests();
            ViewBag.list = list;
            ViewBag.count = list.Count();
            return View("~/Views/Admin/ViewSRRequests.cshtml");
        }

        [HttpPost]
        public ActionResult RejectSRRequest(string Username)
        {
            int numOfRows = db.RespondRequest('R', Username);
            List<string> list = db.SelectSRRequests();
            ViewBag.list = list;
            ViewBag.count = list.Count();
            return View("~/Views/Admin/ViewSRRequests.cshtml");
        }

        public ActionResult ViewSARequests()
        {
            List<string> list = db.SelectSARequests();
            ViewBag.list = list;
            ViewBag.count = list.Count();
            return View();
        }

        [HttpPost]
        public ActionResult AcceptSARequest(string Username)
        {
            int numOfRows = db.RespondRequest('A', Username);
            List<string> list = db.SelectSARequests();
            ViewBag.list = list;
            ViewBag.count = list.Count();
            return View("~/Views/Admin/ViewSARequests.cshtml");
        }

        [HttpPost]
        public ActionResult RejectSARequest(string Username)
        {
            int numOfRows = db.RespondRequest('R', Username);
            List<string> list = db.SelectSARequests();
            ViewBag.list = list;
            ViewBag.count = list.Count();
            return View("~/Views/Admin/ViewSARequests.cshtml");
        }

        public ActionResult AddAdmin()
        {
            return View();
        }

        public string hashPassword(string password)
        {
            const string salt = "r4Nd0m_5A1t";  //They are concatenated to the password to protects against rainbow table attacks.
            HashAlgorithm algorithm = new SHA256Managed();
            string passwordandsalt = password + salt;
            string hashed = Convert.ToBase64String(algorithm.ComputeHash(System.Text.Encoding.UTF8.GetBytes(passwordandsalt)));
            return hashed;
        }

        [HttpPost]
        public ActionResult AddNewAdmin(string Username, string Email, string Password)
        {
            string hashed = hashPassword(Password);
            int numOfRows = db.AddUser(Username, hashed, "Admin", Email, 'A');
            if (numOfRows == 0) return View("~/Views/Admin/AAFail.cshtml");
            return View("~/Views/Admin/AASuccess.cshtml");
        }
    }
}