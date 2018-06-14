using CUFE_SF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;
using System.Net.Mail;

namespace CUFE_SF.Controllers
{
    public class LoginController : Controller
    {
        DBManager db = new DBManager();

        public ActionResult Index()
        {
            ViewBag.Message = "Login page";
            return View();
        }

        public ActionResult ForgotPassword()
        {
            ViewBag.Message = "Fotgot Password";
            return View();
        }
        [HttpGet]
        public ActionResult ResetPassword(string username)
        {
            ViewBag.Message = "Reset Password";
            ViewBag.Username = username;
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
        public ActionResult LoginCheck2(string Password, string UsernameOrEmail)
        {
            string RightPassword = (string)db.CheckLoginInfo(UsernameOrEmail);
            string hashed = hashPassword(Password);
            if (RightPassword != hashed) return View("~/Views/Login/LoginFail.cshtml");
            else
            {
                char request = (char)db.GetRequest(UsernameOrEmail);
                if (request == 'P') return View("~/Views/Register/Pending.cshtml");
                if (request == 'R') return View("~/Views/Register/Rejected.cshtml");
                LoginInfo.LoggedIn = true;
                if (UsernameOrEmail.Contains('@')) LoginInfo.LoginUsername = (string)db.GetUsername(UsernameOrEmail);
                else LoginInfo.LoginUsername = UsernameOrEmail;
                string type = (string)db.GetType(UsernameOrEmail);
                if (type == "SR" && request == 'A')
                {
                    LoginInfo.UserType = "SR";
                    return View("~/Views/SR/Index.cshtml");
                }
                if (type == "SA" && request == 'A')
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

        [HttpPost]
        public ActionResult SendResetEmail(string Email)
        {
            string password = (string)db.CheckLoginInfo(Email);
            if (password == null)
                View("~/Views/Login/LoginFail.cshtml");
            else
            {
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);

                smtpClient.UseDefaultCredentials = true;
                smtpClient.Credentials = new System.Net.NetworkCredential("ahmedyoussry1996@gmail.com", "ahmedyoussry1234");
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.EnableSsl = true;
                MailMessage mail = new MailMessage();
                smtpClient.EnableSsl = true;
                //Setting From , To and CC
                mail.From = new MailAddress("ahmedyoussry1996@gmail.com");
                mail.To.Add("ahmedyoussry1996@gmail.com");
                //mail.CC.Add("omarmah10@gmail.com");
                mail.Subject = "Elsafa7een project";
                string username = (string)db.GetUsername(Email);
                mail.Body = "Click on the following link to reset password \n http://localhost:63835/Login/ResetPassword?username="+username;
                smtpClient.Send(mail);
            }
            return View("~/Views/Login/EmailSent.cshtml");
        }

        [HttpGet]
        public ActionResult ChangePassword(string username, string Password)
        {
            string password = (string)db.CheckLoginInfo(username);
            if(password == null) return View("~/Views/Login/LoginFail.cshtml");
            else
            {
                string hashed = hashPassword(Password);
                int rows = db.ChangePassword(username, hashed);
                if(rows ==0) return View("~/Views/Home/About.cshtml");
                else return View("~/Views/Login/Index.cshtml");
            }
        }
    }
}