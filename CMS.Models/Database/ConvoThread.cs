using CMS.Data.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace CMS.Data.Database
{
    [Table("Threads")]
    public class ConvoThread
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string ThreadTitle { get; set; }
        [Required]
        public string InitialSenderEmail { get; set; }
        public string InitialSenderName { get; set; }
        [Required]
        public DateTime LatestMessageTimestamp { get; set; }
        [Required]
        public DateTime TimestampCreated { get; set; }
        
        [InverseProperty("Thread")]
        public IEnumerable<Email> Emails { get; set; }
    }


    public static partial class ModelExtensions
    {
        public static ThreadViewModel GetViewModel(this ConvoThread thread)
        {
            return new ThreadViewModel
            {
                Id = thread.Id,
                SenderEmail = thread.InitialSenderEmail,
                SenderName = thread.InitialSenderName,
                LatestMessageTimestamp = thread.LatestMessageTimestamp,
                Messages = thread.Emails?.Select(x => x.GetViewModel()),
                Title = thread.ThreadTitle
            };
        }
    }
}
