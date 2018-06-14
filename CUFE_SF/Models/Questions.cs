using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CUFE_SF.Models
{
    [Table("Questions")]
    public class Questions
    {
        [Key]
        public int QID { get; set; }
        public string Username { get; set; }
        public string CourseID { get; set; }
        public string Text { get; set; }
        public string Title { get; set; }
        public string QDate { get; set; } //
        public int Rating { get; set; } //
        public class QuestionDBContext : DbContext
        {
            public QuestionDBContext() : base("CUFESF.Properties.Settings.ConnectionString")
            { }
            public DbSet<Questions> Questions { get; set; }
        }

    }
}