using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CUFE_SF.Models
{ 
     [Table("Students")]
       public class Students
      {
        [Key]
        public string Username { get; set; }
        public int ID { get; set; }
        public string Year { get; set; }
        public string Email { get; set; }
        public class StudentDBContext : DbContext
        {
            public StudentDBContext() : base("CUFESF.Properties.Settings.ConnectionString")
            { }
            public DbSet<Students> Students { get; set; }
        }

    }
}