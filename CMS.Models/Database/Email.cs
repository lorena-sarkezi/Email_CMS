using CMS.Data.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CMS.Data.Database
{
    [Table("Emails", Schema ="dbo")]
    public class Email
    {
        [Required, Key]
        public int Id { get; set; }
        public int ThreadId { get; set; }
        public string Subject { get; set; }
        public DateTime Timestamp { get; set; }
        public string HtmlContent { get; set; }
        public string TextContent { get; set; }
        public string MessageContent { get; set; }


        [InverseProperty("Email")]
        public virtual IEnumerable<Sender> Senders { get; set; }

        [InverseProperty("Email")]

        public virtual IEnumerable<Recepient> Recepients { get; set; }

        public virtual ConvoThread Thread { get; set; }
    }
}
