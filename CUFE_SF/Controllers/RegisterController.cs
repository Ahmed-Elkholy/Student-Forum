using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CUFE_SF.Models;
using System.Security.Cryptography;

namespace CUFE_SF.Controllers
{
    public class RegisterController : Controller
    {
        DBManager db = new DBManager();
        private Users.UsersDBContext db2 = new Users.UsersDBContext();

        public ActionResult Index()
        {
            ViewBag.Message = "Registration page.";
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


        public ActionResult RegisterStudent()
        {
            ViewBag.Message = "Student Registration Page.";
            return View();
        }

   
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterStudent(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            return View(model);
            string hashed = hashPassword(model.Password);
            int numOfRows1 = db.AddUser(model.Username, hashed, "ST",model.Email,'A');
            int numOfRows2 = db.AddStudentInfo(model.Username, model.ID, model.Year, model.Email);
            if (numOfRows1 == 0 || numOfRows2 == 0) return View("~/Views/Register/RegFailed.cshtml");
            return View("~/Views/Register/RegSuccess.cshtml");
        }
      
        [HttpPost]
        public ActionResult AddSR(RegisterViewModelSR model)
        {
            if (!ModelState.IsValid)
                return View(model);
            string hashed = hashPassword(model.Password);
            int numOfRows1 = db.AddUser(model.Username,hashed, "SR",model.Email,'P');
            int numOfRows2 = db.AddSRInfo(model.Username, model.ID, model.Year, model.Email,model.Request);
            if (numOfRows1 == 0 || numOfRows2 == 0) return View("~/Views/Register/RegFailed.cshtml");
            return View("~/Views/Register/RequestSuccess.cshtml");
        }

        [HttpPost]
        public ActionResult AddSA(RegisterViewModelSA model)
        {
            if (!ModelState.IsValid)
                return View(model);
            string hashed = hashPassword(model.Password);
            int numOfRows1 = db.AddUser(model.Username, hashed,"SA",model.Email, 'P');
            int numOfRows2 = db.AddSAInfo(model.Username, model.Email, model.About);
            if (numOfRows1 == 0 || numOfRows2 == 0) return View("~/Views/Register/RegFailed.cshtml");
            return View("~/Views/Register/RequestSuccess.cshtml");
        }

        public ActionResult RegisterSR()
        {
            ViewBag.Message = "Student Representative Registration Page.";
            return View();
        }

        public ActionResult RegisterSA()
        {
            ViewBag.Message = "Student Activity Registration Page.";
            return View();
        }

        public ActionResult RegSuccess()
        {
            ViewBag.Message = "Successful Registration";
            return View();
        }

        public ActionResult RegFailed()
        {
            ViewBag.Message = "Failed Registration";
            return View();
        }
    }
}