using CMS.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Core.Repositories
{
    public interface IEmailRepository
    {
        Task<int> GetThreadsCount();
        Task<List<ThreadViewModel>> FetchThreadsPaged(int start, int end);
        Task<ThreadViewModel> GetEntireThreadById(int threadId);
    }
}
