
using CMS.Data.Database;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Core.Services
{
    public interface IMessageParserService
    {
        Task<Email> ConvertMimeMessageToLocalEmail(MimeMessage mimeMessage);
        Task<ConvoThread> GetConversationThread(string subject);
        void RemoveExtraQuoteRows(ref List<string> filteredTextBodyNoQuotes);
        
    }
}
