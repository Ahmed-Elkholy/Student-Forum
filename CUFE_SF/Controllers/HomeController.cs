using CUFE_SF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;

namespace CUFE_SF.Controllers
{
    public class HomeController : Controller
    {
        DBManager db = new DBManager();

        public ActionResult Index()
        {
            if(LoginInfo.LoggedIn)
            {
                if (LoginInfo.UserType == "Admin") return View("~/Views/Admin/Index.cshtml"); 
                else if (LoginInfo.UserType == "SR") return View("~/Views/SR/Index.cshtml");
                else if (LoginInfo.UserType == "SA") return View("~/Views/SA/Index.cshtml");
                else return View("~/Views/ST/Index.cshtml");
            }
               
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

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
        
        [HttpPost]
        public ActionResult LoginCheck(string Password, string UsernameOrEmail)
        {
            string RightPassword = (string)db.CheckLoginInfo(UsernameOrEmail);
            string hashed = hashPassword(Password);
            if (RightPassword != hashed) return View("~/Views/Login/LoginFail.cshtml");
            else
            {
                string request = (string)db.GetRequest(UsernameOrEmail);
                if (request == "P") return View("~/Views/Register/Pending.cshtml");
                if (request == "R") return View("~/Views/Register/Rejected.cshtml");
                LoginInfo.LoggedIn = true;
                if (UsernameOrEmail.Contains('@')) LoginInfo.LoginUsername = (string)db.GetUsername(UsernameOrEmail);
                else LoginInfo.LoginUsername = UsernameOrEmail;
                string type = (string)db.GetType(UsernameOrEmail);
                if (type == "SR" && request == "A")
                {
                    LoginInfo.UserType = "SR";
                    return View("~/Views/SR/Index.cshtml");
                }
                if (type == "SA" && request == "A")
                {
                    LoginInfo.UserType = "SA";
                    return View("~/Views/SA/Index.cshtml");
                }

                if (type == "Admin")
                {
                    LoginInfo.UserType = "Admin";
                    return View("~/Views/Admin/Index.cshtml");
                }
                LoginInfo.UserType = "ST";
                return View("~/Views/ST/Index.cshtml");
            }
        }
    }
}