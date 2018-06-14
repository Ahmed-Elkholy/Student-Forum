using CUFE_SF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static CUFE_SF.Models.Students;
using static CUFE_SF.Models.Users;

namespace CUFE_SF.Controllers
{
    public class ValidationController : Controller
    {
        public JsonResult IsIDExist(int ID)
        {
            StudentDBContext DB = new StudentDBContext();
            bool isExist = (DB.Students.Any(x => x.ID == ID));
            return Json(!isExist, JsonRequestBehavior.AllowGet);
        }



        public JsonResult IsUserNameExist(string userName)
        {
            UsersDBContext DB = new UsersDBContext();
            bool isExist = (DB.Users.Any(x => x.Username == userName));
            return Json(!isExist, JsonRequestBehavior.AllowGet); 
        }
       
        public JsonResult IsEmailExist(string Email)
        {
            StudentDBContext DB = new StudentDBContext();
            bool isExist = (DB.Students.Any(x => x.Email == Email));
            return Json(!isExist, JsonRequestBehavior.AllowGet);
        }
    }
}