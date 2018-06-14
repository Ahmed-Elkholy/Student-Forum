using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace CUFE_SF.Models
{
    public class DBManager
    {
        SqlConnection con;
        public DBManager()
        {
            con = new SqlConnection(WebConfigurationManager.ConnectionStrings["CUFESF.Properties.Settings.ConnectionString"].ConnectionString);
            con.Open();
            
        }

        public int AddUser(string Username, string Password, string Type, string Email,char Request_Status)
        {
            SqlCommand sqlcommand = new SqlCommand("AddUser", con);
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@Username", Username);
            Parameters.Add("@Password", Password);
            Parameters.Add("@Type", Type);
            Parameters.Add("@Email", Email);
            Parameters.Add("@Request_Status", Request_Status);
            return ExecuteNonQuery("AddUser", Parameters);
        }

        public int AddCourse(string Code, string Name, string Username)
        {
            SqlCommand sqlcommand = new SqlCommand("AddCourse", con);
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@Code", Code);
            Parameters.Add("@Name", Name);
            Parameters.Add("@Username", Username);
            return ExecuteNonQuery("AddCourse", Parameters);
        }

        public int AddSRAnnoucement(string Code, string text, string Username,string title)
        {
            SqlCommand sqlcommand = new SqlCommand("AddAnnouncement", con);
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@Course", Code);
            Parameters.Add("@text", text);
            Parameters.Add("@Username", Username);
            Parameters.Add("@title", title);
            return ExecuteNonQuery("AddAnnouncement", Parameters);
        }

        public int ChangePassword(string Email, string Password)
        {
            SqlCommand sqlcommand = new SqlCommand("ChangePassword", con);
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@Email", Email);
            Parameters.Add("@Password", Password);
            return ExecuteNonQuery("ChangePassword", Parameters);
        }

        public List<string> SelectCoursesBySR(string Username)
        {
            SqlCommand sqlcommand = new SqlCommand("SelectCoursesBySR", con);
            List<string> list = new List<string>();
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@Username", Username);
            SqlDataReader reader = ExecuteReader("SelectCoursesBySR", Parameters);
            if (reader != null)
            {
                while (reader.Read())
                {
                    list.Add(Convert.ToString(reader["Code"]));
                }
            }
            return list;
        }

        public List<string> SelectCourses()
        {
            List<string> list = new List<string>();
            SqlCommand sqlcommand = new SqlCommand("SelectCourses", con);
            SqlDataReader reader = sqlcommand.ExecuteReader();
            while (reader.Read())
            {
                list.Add(Convert.ToString(reader["Code"]));
            }
            return list;
        }

        public List<string> SelectAllEvents()
        {
            List<string> list = new List<string>();
            SqlCommand sqlcommand = new SqlCommand("SelectAllEvents", con);
            SqlDataReader reader = sqlcommand.ExecuteReader();
            while (reader.Read())
            {
                list.Add(Convert.ToString(reader["Username"]));
                list.Add(Convert.ToString(reader["EName"]));
                list.Add(Convert.ToString(reader["Date"]));
                list.Add(Convert.ToString(reader["Fee"]));
                list.Add(Convert.ToString(reader["Description"]));
                list.Add(Convert.ToString(reader["Location"]));
            }
            return list;
        }

        public List<string> SelectCoursesEnrolled(string username)
        {
            List<string> list = new List<string>();
            SqlCommand sqlcommand = new SqlCommand("SelectCoursesEnrolled", con);
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@Username", username);
            SqlDataReader reader = ExecuteReader("SelectCoursesEnrolled", Parameters);
            if(reader != null)
            {
                while (reader.Read())
                {
                    list.Add(Convert.ToString(reader["Code"]));
                }
            }
             return list;
        }

        public List<string> SelectEvents(string username)
        {
            List<string> list = new List<string>();
            SqlCommand sqlcommand = new SqlCommand("SelectEvents", con);
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@Username", username);
            SqlDataReader reader = ExecuteReader("SelectEvents", Parameters);
            if (reader != null)
            {
                while (reader.Read())
                {
                    list.Add(Convert.ToString(reader["EName"]));
                }
            }
            return list;
        }

        public List<string> SelectEvent(string EName)
        {
            List<string> list = new List<string>();
            SqlCommand sqlcommand = new SqlCommand("SelectEvent", con);
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@EName", EName);
            SqlDataReader reader = ExecuteReader("SelectEvent", Parameters);
            if (reader != null)
            {
                reader.Read();
                list.Add(Convert.ToString(reader["Username"]));
                list.Add(Convert.ToString(reader["EName"]));
                list.Add(Convert.ToString(reader["Date"]));
                list.Add(Convert.ToString(reader["Fee"]));
                list.Add(Convert.ToString(reader["Description"]));
                list.Add(Convert.ToString(reader["Location"]));
            }
            return list;
        }

        public List<string> SelectReports(string Username)
        {
            List<string> list = new List<string>();
            SqlCommand sqlcommand = new SqlCommand("SelectReportsForSR", con);
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@Username", Username);
            SqlDataReader reader = ExecuteReader("SelectReportsForSR", Parameters);
            if (reader != null)
            {
                reader.Read();
                list.Add(Convert.ToString(reader["Course"]));
                list.Add(Convert.ToString(reader["Title"]));
                list.Add(Convert.ToString(reader["Text"]));
                list.Add(Convert.ToString(reader["AskedBy"]));
                list.Add(Convert.ToString(reader["ReportedBy"]));
                list.Add(Convert.ToString(reader["QuestionID"]));

            }
            return list;
        }

        public List<string> SelectSRRequests()
        {
            List<string> list = new List<string>();
            SqlCommand sqlcommand = new SqlCommand("SelectSRRequests", con);
            SqlDataReader reader = sqlcommand.ExecuteReader();
            
            
            while (reader.Read())
            {
                list.Add(Convert.ToString(reader["Username"]));
                              
            }
            reader.Close();
            List<string> list2 = SelectSR(list);
            return list2;
        }

        public List<string> SelectSR(List<string> Usernames)
        {
            List<string> list = new List<string>();
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            SqlCommand sqlcommand = new SqlCommand("SelectSR", con);
            foreach (string x in Usernames)
            {
                Parameters.Add("@Username", x);
                SqlDataReader reader = ExecuteReader("SelectSR", Parameters);
                if (reader != null)
                {
                    reader.Read();
                    list.Add(Convert.ToString(reader["Username"]));
                    list.Add(Convert.ToString(reader["ID"]));
                    list.Add(Convert.ToString(reader["Email"]));
                    list.Add(Convert.ToString(reader["Year"]));
                    list.Add(Convert.ToString(reader["Request"]));
                }
                Parameters.Remove("@Username");
                reader.Close();
            }
                  
                     
            return list;
        }

        public List<string> SelectSARequests()
        {
            List<string> list = new List<string>();
            SqlCommand sqlcommand = new SqlCommand("SelectSARequests", con);
            SqlDataReader reader = sqlcommand.ExecuteReader();


            while (reader.Read())
            {
                list.Add(Convert.ToString(reader["Username"]));

            }
            reader.Close();
            List<string> list2 = SelectSA(list);
            return list2;
        }

        public List<string> SelectSA(List<string> Usernames)
        {
            List<string> list = new List<string>();
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            SqlCommand sqlcommand = new SqlCommand("SelectSA", con);
            foreach (string x in Usernames)
            {
                Parameters.Add("@Username", x);
                SqlDataReader reader = ExecuteReader("SelectSA", Parameters);
                if (reader != null)
                {
                    reader.Read();
                    list.Add(Convert.ToString(reader["Username"]));
                    list.Add(Convert.ToString(reader["Email"]));
                    list.Add(Convert.ToString(reader["About"]));
                }
                Parameters.Remove("@Username");
                reader.Close();
            }
            return list;
        }

        public List<string> SelectCourseAnnouncements(string Course)
        {
            List<string> list = new List<string>();
            SqlCommand sqlcommand = new SqlCommand("SelectCourseAnnouncements", con);
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@Course", Course);
            SqlDataReader reader = ExecuteReader("SelectCourseAnnouncements", Parameters);
            if (reader != null)
            {
                while (reader.Read())
                {
                    list.Add(Convert.ToString(reader["Title"]));
                    list.Add(Convert.ToString(reader["Post"]));
                }
                reader.Close();
                return list;
            }
            else return null; 
        }

        public int RemoveCourse(string Code)
        {
            SqlCommand sqlcommand = new SqlCommand("RemoveCourse", con);
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@Code", Code);
            return ExecuteNonQuery("RemoveCourse", Parameters);
        }

        public int RemoveEvent(string Username, string EName)
        {
            SqlCommand sqlcommand = new SqlCommand("RemoveEvent", con);
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@Username", Username);
            Parameters.Add("@EName", EName);
            return ExecuteNonQuery("RemoveEvent", Parameters);
        }

        public int UpdateCourse(string Code, string Name)
        {
            SqlCommand sqlcommand = new SqlCommand("UpdateCourse", con);
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@Code", Code);
            Parameters.Add("@Name", Name);
            return ExecuteNonQuery("UpdateCourse", Parameters);
        }

        public int UpdateEvent(string Username, string Ename, string Date, string Fee, string Description, string Location)
        { 
            SqlCommand sqlcommand = new SqlCommand("UpdateEvent", con);
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@Username", Username);
            Parameters.Add("@Ename", Ename);
            Parameters.Add("@Date", Date);
            Parameters.Add("@Fee", Fee);
            Parameters.Add("@Description", Description);
            Parameters.Add("@Location", Location);
            return ExecuteNonQuery("UpdateEvent", Parameters);
        }

        public int RespondRequest(char Response, string Name)
        {
            SqlCommand sqlcommand = new SqlCommand("RespondRequest", con);
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@Response", Response);
            Parameters.Add("@Username", Name);
            return ExecuteNonQuery("RespondRequest", Parameters);
        }

        public int AddStudentInfo(string Username, long ID, string Year, string Email)
        {
            SqlCommand sqlcommand = new SqlCommand("AddStudent", con);
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@Username", Username);
            Parameters.Add("@ID", ID);
            Parameters.Add("@Year", Year);
            Parameters.Add("@Email", Email);
            return ExecuteNonQuery("AddStudent", Parameters);
        }

        public int AddSRInfo(string Username, long ID, string Year, string Email,string Request)
        {
            SqlCommand sqlcommand = new SqlCommand("AddSR", con);
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@Username", Username);
            Parameters.Add("@ID", ID);
            Parameters.Add("@Year", Year);
            Parameters.Add("@Email", Email);
            Parameters.Add("@Request", Request);
            return ExecuteNonQuery("AddSR", Parameters);
        }

        public int AddSAInfo(string Username, string Email, string About)
        {
            SqlCommand sqlcommand = new SqlCommand("AddSA", con);
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@Username", Username);
            Parameters.Add("@Email", Email);
            Parameters.Add("@About", About);
            return ExecuteNonQuery("AddSA", Parameters);
        }

        public object CheckLoginInfo(string UsernameOrEmail)
        {
            SqlCommand sqlcommand = new SqlCommand("GetPassword", con);
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@UsernameOrEmail", UsernameOrEmail);
            return ExecuteScalar("GetPassword", Parameters);
        }

        public object GetType(string UsernameOrEmail)
        {
            SqlCommand sqlcommand = new SqlCommand("GetType", con);
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@UsernameOrEmail", UsernameOrEmail);
            return ExecuteScalar("GetType", Parameters);
        }

        public object GetRequest(string UsernameOrEmail)
        {
            SqlCommand sqlcommand = new SqlCommand("GetRequestStatus", con);
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@Username", UsernameOrEmail);
            return ExecuteScalar("GetRequestStatus", Parameters);
        }

        public object GetUsername(string Email)
        {
            SqlCommand sqlcommand = new SqlCommand("GetUsernameFromEmail", con);
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@Email", Email);
            return ExecuteScalar("GetUsernameFromEmail", Parameters);
        }

        public int AddQuestion(string Username, string text, string Course, string title) //
        {
            SqlCommand sqlcommand = new SqlCommand("AddQuestion", con);
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@Name", Username);
            Parameters.Add("@text", text);
            Parameters.Add("@CourseID", Course);
            Parameters.Add("@Title", title);
            Parameters.Add("@QDate", DateTime.Now);
            Parameters.Add("@Rating", 0);
            return ExecuteNonQuery("AddQuestion", Parameters);
        }

        public int EnrollinCourse(string Code, string Username)
        {
            SqlCommand sqlcommand = new SqlCommand("EnrollinCourse", con);
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@Code", Code);
            Parameters.Add("@Username", Username);
            return ExecuteNonQuery("EnrollinCourse", Parameters);
        }

        public int AddEvent(string Username, string Ename, string Date, string Fee, string Description, string Location)
        {
            SqlCommand sqlcommand = new SqlCommand("AddEvent", con);
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@Username", Username);
            Parameters.Add("@Ename", Ename);
            Parameters.Add("@Date", Date);
            Parameters.Add("@Fee", Fee);
            Parameters.Add("@Description", Description);
            Parameters.Add("@Location", Location);
            return ExecuteNonQuery("AddEvent", Parameters);
        }

        public int UpdateRating(int QID, int newRating) //
        {
            SqlCommand sqlcommand = new SqlCommand("UpdateRating", con);
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@QID", QID);
            Parameters.Add("@Rating", newRating);
            return ExecuteNonQuery("UpdateRating", Parameters);
        }

        public int GetRating(int QID) //
        {
            SqlCommand sqlcommand = new SqlCommand("GetRating", con);
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@QID", QID);
            return (int)ExecuteScalar("GetRating", Parameters);
        }

        public int AddUserRating(int QID, string Username, bool Rating) //
        {
            SqlCommand sqlcommand = new SqlCommand("AddUserRating", con);
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@QID", QID);
            Parameters.Add("@Username", Username);
            Parameters.Add("@Rating", Rating);
            return ExecuteNonQuery("AddUserRating", Parameters);
        }

        public int UpdateUserRating(int QID, string Username, bool Rating) //
        {
            SqlCommand sqlcommand = new SqlCommand("UpdateUserRating", con);
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@QID", QID);
            Parameters.Add("@Username", Username);
            Parameters.Add("@Rating", Rating);
            return ExecuteNonQuery("UpdateUserRating", Parameters);
        }

        public Object SelectUserRating(int QID, string Username) //
        {
            SqlCommand sqlcommand = new SqlCommand("SelectUserRating", con);
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@QID", QID);
            Parameters.Add("@Username", Username);
            return ExecuteScalar("SelectUserRating", Parameters);
        }

        public int AddReport(int QID, string Username,string Code) //
        {
            SqlCommand sqlcommand = new SqlCommand("AddReport", con);
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@QID", QID);
            Parameters.Add("@Username", Username);
            Parameters.Add("@Course", Code);
            return ExecuteNonQuery("AddReport", Parameters);
        }

        public int DeleteQuestion(int QID) //
        {
            SqlCommand sqlcommand = new SqlCommand("DeleteQuestion", con);
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@QID", QID);
            return ExecuteNonQuery("DeleteQuestion", Parameters);
        }
        public int DeleteReport(int QID) //
        {
            SqlCommand sqlcommand = new SqlCommand("DeleteReport", con);
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@QID", QID);
            return ExecuteNonQuery("DeleteReport", Parameters);
        }

        public Object SelectUserReport(int QID, string Username) //
        {
            SqlCommand sqlcommand = new SqlCommand("SelectUserReport", con);
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@QID", QID);
            Parameters.Add("@Username", Username);
            return ExecuteScalar("SelectUserReport", Parameters);
        }

        public SqlDataReader SelectCourseDetails(string Code) //
        {
            SqlCommand sqlcommand = new SqlCommand("SelectCourseDetails", con);
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@Code", Code);
            return ExecuteReader("SelectCourseDetails", Parameters);
        }

        public int ExecuteNonQuery(string storedProcedureName, Dictionary<string, object> parameters)
        {
            try
            {
                SqlCommand myCommand = new SqlCommand(storedProcedureName, con);

                myCommand.CommandType = CommandType.StoredProcedure;

                foreach (KeyValuePair<string, object> Param in parameters)
                {
                    myCommand.Parameters.Add(new SqlParameter(Param.Key, Param.Value));
                }

                return myCommand.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }

        public object ExecuteScalar(string storedProcedureName, Dictionary<string, object> parameters)
        {
            try
            {
                SqlCommand myCommand = new SqlCommand(storedProcedureName, con);

                myCommand.CommandType = CommandType.StoredProcedure;

                if (parameters != null)
                {
                    foreach (KeyValuePair<string, object> Param in parameters)
                    {
                        myCommand.Parameters.Add(new SqlParameter(Param.Key, Param.Value));
                    }
                }

                return myCommand.ExecuteScalar();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public SqlDataReader ExecuteReader(string storedProcedureName, Dictionary<string, object> parameters)
        {
            try
            {
                SqlCommand myCommand = new SqlCommand(storedProcedureName, con);

                myCommand.CommandType = CommandType.StoredProcedure;

                if (parameters != null)
                {
                    foreach (KeyValuePair<string, object> Param in parameters)
                    {
                        myCommand.Parameters.Add(new SqlParameter(Param.Key, Param.Value));
                    }
                }

                SqlDataReader reader = myCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    return reader;
                }
                else
                {
                    reader.Close();
                    return null;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}