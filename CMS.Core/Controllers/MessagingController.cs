using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS.Core.Models;
using CMS.Core.Repositories;
using CMS.Core.Services;
using CMS.Data.Common;
using CMS.Data.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Core.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MessagingController : ControllerBase
    {
        private readonly IEmailService emailService;
        private readonly IEmailRepository emailRepository;

        public MessagingController(IEmailService emailService, IEmailRepository emailRepository)
        {
            this.emailService = emailService;
            this.emailRepository = emailRepository;
        }        

        //[HttpGet("fetch")]
        //public async Task<List<MailThreadBasic>> FetchEmail()
        //{
        //    return await emailService.ReceiveAndProcessMail();
        //}

        [HttpGet("threads")]
        public async Task<List<ThreadViewModel>> GetMailThreadsListPaged([FromQuery]int start, [FromQuery]int end)
        {
            await emailService.ReceiveAndProcessMail(100); //Process latest 100 emails on server and save to DB
            return await emailRepository.FetchThreadsPaged(start, end);
        }

        [HttpGet("threads/count")]
        public async Task<int> GetThreadsCount()
        {
            return await emailRepository.GetThreadsCount();
        }

        [HttpGet("threads/{threadId}")]
        public async Task<ThreadViewModel> GetThread([FromRoute] int threadId)
        {
            return await emailRepository.GetEntireThreadById(threadId);
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage(SendMessageModel model)
        {
            Email email = await emailService.ComposeEmail(model.ThreadId, model.MessageContent);
            await emailService.SendMail(email);

            return Ok();
        }
    }
}