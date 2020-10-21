using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CMS.Models.Database
{
    [Table("emails", Schema ="dbo")]
    public class Email
    {
        [Required, Key]
        public int Id { get; set; }
        public string Subject { get; set; }
        public DateTime Timestamp { get; set; }
        public string SenderEmail { get; set; }
        public string SenderName { get; set; }
        public string RecepientEmail { get; set; }
        public string RecepientName { get; set; }
        public string Content { get; set; }
    }
}
