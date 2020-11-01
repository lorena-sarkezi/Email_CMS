using System;
using System.Collections.Generic;
using System.Text;

namespace CMS.Data.Common
{
    public class ThreadViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public DateTime LatestMessageTimestamp { get; set; }
        public IEnumerable<MessageViewModel> Messages { get; set; }
    }
}
