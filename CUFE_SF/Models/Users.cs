using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CUFE_SF.Models
{
    [Table("Users")]
    public class Users
    {
        [Key]
        public string Username { get; set; }
        public string Password { get; set; }
        public string Type { get; set; }
        public string Email { get; set; }
        public class UsersDBContext : DbContext
        {
            public UsersDBContext() :base("CUFESF.Properties.Settings.ConnectionString")
            { }
            public DbSet<Users> Users { get; set; }
        }


    }
}