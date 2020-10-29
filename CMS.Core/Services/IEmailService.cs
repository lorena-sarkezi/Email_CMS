using CMS.Core.Models;
using CMS.Data.Common;
using CMS.Data.Database;
using CMS.Data.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Core.Services
{
    public interface IEmailService
    {
        bool SendMail(Email email);
        Task<List<MailThreadBasic>> ReceiveMail(int count = 10);
        Task<List<MailThreadBasic>> GetBasicMailThreads();

        public Task<ThreadViewModel> GetThread(int threadId);
    }
}
