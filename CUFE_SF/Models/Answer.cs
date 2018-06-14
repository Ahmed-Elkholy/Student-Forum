using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CUFE_SF.Models
{
    [Table("Answer")]
    public class Answer
    {
        public int QID { get; set; }
        public string UserName{ get; set; }
        public string Text { get; set; }
        public string ADate { get; set; } //
        //public int ARating { get; set; } //
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AID { get; set; }
        public class AnswerDBContext : DbContext
        {
            public AnswerDBContext() : base("CUFESF.Properties.Settings.ConnectionString")
            { }
            public DbSet<Answer> Answers { get; set; }
        }


    }
}