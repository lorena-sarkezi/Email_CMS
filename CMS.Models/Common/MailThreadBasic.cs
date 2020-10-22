using CMS.Models.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace CMS.Data.Common
{
    public class MailThreadBasic
    {
        public string Subject { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public static partial class ModelExtensions
    {
        public static MailThreadBasic ToMailThreadBasic(this Email email)
        {
            return new MailThreadBasic
            {
                Subject = email.Subject,
                Timestamp = email.Timestamp,
                SenderEmail = email.SenderEmail,
                SenderName = email.SenderName
            };
        }
    }
    
}
