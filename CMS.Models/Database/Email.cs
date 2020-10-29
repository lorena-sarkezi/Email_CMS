using CMS.Data.Common;
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
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ThreadId { get; set; }
        public string Subject { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsIncoming { get; set; }
        public string HtmlContent { get; set; }
        public string TextContent { get; set; }
        public string MessageContent { get; set; }


        [InverseProperty("Email")]
        public virtual IEnumerable<Sender> Senders { get; set; }

        [InverseProperty("Email")]

        public virtual IEnumerable<Recepient> Recepients { get; set; }

        public virtual ConvoThread Thread { get; set; }
    }

    public static partial class ModelExtensions
    {
        public static MessageViewModel GetViewModel(this Email email)
        {
            return new MessageViewModel
            {
                MessageContent = email.MessageContent,
                IsOwnMessage = !email.IsIncoming,
                Timestamp = email.Timestamp
            };
        }
    }
}
