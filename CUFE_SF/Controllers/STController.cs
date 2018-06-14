using CUFE_SF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static CUFE_SF.Models.Answer;
using static CUFE_SF.Models.Questions;

namespace CUFE_SF.Controllers
{
    public class STController : Controller
    {
        DBManager db = new DBManager();
        
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult Enroll(string Code)
        {
            List<string> list = db.SelectCourses();
            ViewBag.list = list; //code list
            return View();
        }

        [HttpPost]
        public ActionResult ChooseCourse(string Code)
        {
            int noOfrows = db.EnrollinCourse(Code, LoginInfo.LoginUsername);
            List<string> list = db.SelectCourses();
            ViewBag.list = list; //code list
            if (noOfrows == 0) return View("~/Views/ST/EnrollFail.cshtml");
            return View("~/Views/ST/EnrollSuccess.cshtml");
        }

        public ActionResult ViewQuestions(string Code)
        {
            QuestionDBContext DB = new QuestionDBContext();
            LoginInfo.Course = Code;
            List<Questions> set = DB.Questions.ToList<Questions>();
            set = set.Where(b => b.CourseID == LoginInfo.Course).ToList<Questions>();
            set = set.OrderByDescending(b => Convert.ToDateTime(b.QDate)).ToList<Questions>(); //
            ViewBag.list = set;
            ViewBag.course = Code;
            return View();
        }

        [HttpGet]
        public ActionResult ViewQuestion(int question,string Code) //
        {
            QuestionDBContext QDB = new QuestionDBContext();
            List<Questions> qset = QDB.Questions.ToList<Questions>();
            Questions ques = qset.Where(b => b.QID == question).First();
            AnswerDBContext ADB = new AnswerDBContext(); //
            List<Answer> aset = ADB.Answers.ToList<Answer>(); //
            aset = aset.Where(a => a.QID == question).ToList<Answer>(); //
            aset = aset.OrderByDescending(a => Convert.ToDateTime(a.ADate)).ToList<Answer>(); //
            ViewBag.Answers = aset; //
            Object rating = db.SelectUserRating(question, LoginInfo.LoginUsername); //
            if (rating != null) ViewBag.URating = (bool)rating; //
            else ViewBag.Rating = null; //
            ViewBag.Reported = db.SelectUserReport(question, LoginInfo.LoginUsername);
            ViewBag.Question = ques;
            Answer answer = new Answer();
            answer.QID = question;
            ViewBag.course = Code;
            return View(answer);
        }
                
        [HttpPost]
        public ActionResult AddAnswer(Answer answer2)
        {
            AnswerDBContext DB = new AnswerDBContext();
            answer2.UserName = LoginInfo.LoginUsername;
            answer2.ADate = DateTime.Now.ToString();
            DB.Answers.Add(answer2);
            DB.SaveChanges();
            return View("~/Views/ST/AnswerSuccess.cshtml");
        }

        public ActionResult ChooseCourseEnrolled()
        {
            List<string> list = db.SelectCoursesEnrolled(LoginInfo.LoginUsername);
            ViewBag.list = list;
            return View(); 
        }

        public ActionResult ChooseCourseToViewAnnouncements()
        {
            List<string> list = db.SelectCoursesEnrolled(LoginInfo.LoginUsername);
            ViewBag.list = list;
            return View();
        }
        
        [HttpGet]
        public ActionResult ViewCourseAnnouncements(string Course)
        {
            List<string> list = db.SelectCourseAnnouncements(Course);
            ViewBag.list = list;
            if (list!=null) ViewBag.count = list.Count;
            return View();
        }

        public ActionResult ChooseCourseToAnswerPost()
        {
            List<string> list = db.SelectCoursesEnrolled(LoginInfo.LoginUsername);
            ViewBag.list = list;
            return View();
        }

        [HttpPost]
        public ActionResult ChooseCourseToAsk(string Code, string Name)
        {
            LoginInfo.Course = Code;
            return View("~/Views/PostQuestion/Post.cshtml");
        }

        [HttpGet]
        public ActionResult ChooseCourseToAnswer(string Code, string Name)
        {
            LoginInfo.Course = Code;
            return View("~/Views/ST/ViewQuestions.cshtml");
        }
        
        [HttpPost]
        public ActionResult PostAQuestion(string question, string title)
        {
            int numOfRows = db.AddQuestion(LoginInfo.LoginUsername, question, LoginInfo.Course, title);
            if (numOfRows == 0) return View("~/Views/PostQuestion/PostFail.cshtml");
            return View("~/Views/PostQuestion/PostSuccess.cshtml");

        }

        [HttpGet]
        public ActionResult RateQUp(int question) //
        {
            Object rating = db.SelectUserRating(question, LoginInfo.LoginUsername);
            if (rating != null)
            {
                db.UpdateUserRating(question, LoginInfo.LoginUsername, true);
                db.UpdateRating(question, db.GetRating(question) + 2);
            }
            else
            {
                db.AddUserRating(question, LoginInfo.LoginUsername, true);
                db.UpdateRating(question, db.GetRating(question) + 1);
            }
            return Redirect("/ST/ViewQuestion?question="+question.ToString());
        }

        [HttpGet]
        public ActionResult RateQDown(int question) //
        {
            Object rating = db.SelectUserRating(question, LoginInfo.LoginUsername);
            if (rating != null)
            {
                db.UpdateUserRating(question, LoginInfo.LoginUsername, false);
                db.UpdateRating(question, db.GetRating(question) - 2);
            }
            else
            {
                db.AddUserRating(question, LoginInfo.LoginUsername, false);
                db.UpdateRating(question, db.GetRating(question) - 1);
            }
            return Redirect("/ST/ViewQuestion?question=" + question.ToString());
        }

        [HttpGet]
        public ActionResult Report(int question,string Code) //
        {
            db.AddReport(question, LoginInfo.LoginUsername,Code);
            return Redirect("/ST/ViewQuestion?question=" + question.ToString());
        }
    }
}