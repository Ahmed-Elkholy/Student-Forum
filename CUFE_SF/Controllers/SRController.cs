using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CUFE_SF.Models;

namespace CUFE_SF.Controllers
{
    public class SRController : Controller
    {
        DBManager db = new DBManager();
        
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddCourse(string Code, string Name)
        {
            int numOfRows = db.AddCourse(Code, Name, LoginInfo.LoginUsername);
            if (numOfRows == 0) return View("~/Views/SR/CAFail.cshtml");
            return View("~/Views/SR/CASuccess.cshtml");
        }

        public ActionResult DeleteCourse()
        {
            List<string> list = db.SelectCourses();
            ViewBag.list = list; 
            return View(); 
        }

        public ActionResult UpdateCourse()
        {
            List<string> list = db.SelectCourses();
            ViewBag.list = list; 
            return View(); 
        }

       
        public ActionResult ViewCourses()
        {
            List<string> list = db.SelectCoursesBySR(LoginInfo.LoginUsername); 
            ViewBag.list = list;
            return View();
        }

        public ActionResult ViewCoursesToPost()
        {
            List<string> list = db.SelectCoursesBySR(LoginInfo.LoginUsername);
            ViewBag.list = list;
            return View();
        }

        public ActionResult ViewReports()
        {
            List<string> list = db.SelectReports(LoginInfo.LoginUsername);
            ViewBag.list = list;
            ViewBag.count = list.Count;
            return View();
        }
        [HttpGet]
        public ActionResult PostAnnouncement(string Code)
        {
            ViewBag.code = Code;
            return View();
        }

        [HttpGet]
        public ActionResult AddAnnouncement(string Code,string text,string title)
        {
            int rows = db.AddSRAnnoucement(Code, text, LoginInfo.LoginUsername, title);
            if(rows==0) return View("~/Views/SR/AnnFail.cshtml");
            return View("~/Views/SR/PostAdded.cshtml");
        }

        [HttpGet]
        public ActionResult SelectCourseDetails(string Code)
        {
            System.Data.SqlClient.SqlDataReader reader = db.SelectCourseDetails(Code);
            List<string> Usernames = new List<string>();
            List<string> Emails = new List<string>();
            bool empty = false;
            if (reader == null) empty = true;
            else
            {
                while (reader.Read())
                {
                    Usernames.Add(Convert.ToString(reader["Username"]));
                    Emails.Add(Convert.ToString(reader["Email"]));
                }
            }
                     
            ViewBag.Usernames = Usernames;
            ViewBag.Emails = Emails;
            ViewBag.Size = Usernames.Count;
            ViewBag.Empty = empty;
            
            return View("~/Views/SR/ViewCourseDetails.cshtml");
        }

        [HttpPost]
        public ActionResult ModifyCourse(string Code, string Name)
        {
            db.UpdateCourse(Code, Name);
            List<string> list = db.SelectCourses();
            ViewBag.list = list; //code list
            return View("~/Views/SR/UpdateCourse.cshtml");
        }

        [HttpPost]
        public ActionResult RemoveCourse(string Code)
        {
            db.RemoveCourse(Code);
            List<string> list = db.SelectCourses();
            ViewBag.list = list; //code list
            return View("~/Views/SR/DeleteCourse.cshtml");
        }

        [HttpPost]
        public ActionResult UpdateCourse(string Code, string Name)
        {
            db.UpdateCourse(Code, Name);
            List<string> list = db.SelectCourses();
            ViewBag.list = list; //code list
            return View("~/Views/SR/UpdateCourse.cshtml");
        }

        public ActionResult CreateCourse()
        {
            ViewBag.Message = "Create Course";
            return View();
        }

        public ActionResult CASuccess()
        {
            ViewBag.Message = "Course Adding Success";
            return View();
        }

        public ActionResult CAFail()
        {
            ViewBag.Message = "Course Adding Failure";
            return View();
        }
        [HttpPost]
        public ActionResult DeleteQuestion(string QID)
        {
            //int rows2 = db.DeleteReport(Convert.ToInt32(QID));
            int rows = db.DeleteQuestion(Convert.ToInt32(QID));
            if ( rows==0) return View("~/Views/SR/QuestionDelFail.cshtml");
            return View("~/Views/SR/ViewReports.cshtml");
            
        }
        [HttpPost]
        public ActionResult DeleteReoprt(string QID)
        {
            int rows = db.DeleteReport(Convert.ToInt32(QID));
            
            if (rows == 0) return View("~/Views/SR/QuestionDelFail.cshtml");
            return View("~/Views/SR/ViewReports.cshtml");

        }


    }
}