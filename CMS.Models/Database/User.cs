using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CMS.Data.Database
{
    [Table("Users")]
    public class User
    {

        [Key, Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public string PasswordSalt { get; set; }


        [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime TimestampCreated { get; set; }
    }
}
