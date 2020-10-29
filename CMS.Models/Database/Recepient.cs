using CMS.Data.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CMS.Data.Database
{
    [Table("Recepients")]
    public class Recepient
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, ForeignKey("Email")]
        public int EmailId { get; set; }
        public string RecepientName { get; set; }
        [Required]
        public string RecepientEmail { get; set; }

        public virtual Email Email { get; set; }
    }
}
