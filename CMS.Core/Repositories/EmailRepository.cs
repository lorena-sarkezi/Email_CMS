using CMS.Data;
using CMS.Data.Common;
using CMS.Data.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Core.Repositories
{
    public class EmailRepository : IEmailRepository
    {
        private readonly CmsDbContext cmsDbContext;

        public EmailRepository(CmsDbContext cmsDbContext)
        {
            this.cmsDbContext = cmsDbContext;
        }

        public async Task<int> GetThreadsCount()
        {
            return await cmsDbContext.Threads.CountAsync();
        }

        public async Task<ThreadViewModel> GetEntireThreadById(int threadId)
        {
            ConvoThread thread = await cmsDbContext.Threads
                                                   .Include(x => x.Emails)
                                                   .ThenInclude(x => x.Senders)
                                                   .Include(x => x.Emails)
                                                   .ThenInclude(x => x.Recepients)
                                                   .OrderByDescending(x => x.LatestMessageTimestamp)
                                                   .FirstOrDefaultAsync(x => x.Id == threadId);
            if(thread != null)
            {
                thread.Emails = thread?.Emails?.OrderByDescending(x => x.Timestamp);
            }
            
            return thread?.GetViewModel();
        }

        public async Task<List<ThreadViewModel>> FetchThreadsPaged(int start, int end)
        {
            List<ConvoThread> threads = await cmsDbContext.Threads
                                                          .Skip(start)
                                                          .Take(end)
                                                          .OrderByDescending(x => x.LatestMessageTimestamp)
                                                          .ToListAsync();
            List<ThreadViewModel> viewThreads = threads.Select(x => x.GetViewModel()).ToList();
            
            return viewThreads;
        }
    }
}
