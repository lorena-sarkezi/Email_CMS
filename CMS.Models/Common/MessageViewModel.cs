using System;
using System.Collections.Generic;
using System.Text;

namespace CMS.Data.Common
{
    public class MessageViewModel
    {
        public string MessageContent { get; set; }
        public string HtmlContent { get; set; }
        public string SenderEmail { get; set; }
        public string SenderName { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsOwnMessage { get; set; } // If message is from this CMS 
    }
}
