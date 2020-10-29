using System;
using System.Collections.Generic;
using System.Text;

namespace CMS.Data.Common
{
    public class MessageViewModel
    {
        public string MessageContent { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsOwnMessage { get; set; } // If message is from this CMS 
    }
}
