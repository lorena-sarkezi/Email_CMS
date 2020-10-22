using CMS.Core.Models;
using CMS.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Core.Services
{
    public interface IEmailService
    {
        bool SendMail(EmailMessage emailMessage);
        Task<List<EmailMessage>> ReceiveMail(int count = 10);
        Task<List<MailThreadBasic>> GetBasicMailThreads();
    }
}
