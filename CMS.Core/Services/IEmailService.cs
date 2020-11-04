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
        Task<Email> ComposeEmail(int threadId, string content);
        Task<bool> SendMail(Email email);
        Task<List<MailThreadBasic>> ReceiveMail(int count = 10);
        Task<List<ThreadViewModel>> GetThreadsListPaged();

        public Task<ThreadViewModel> GetThread(int threadId);
    }
}
