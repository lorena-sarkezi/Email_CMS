using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS.Core.Models;
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

        public MessagingController(IEmailService emailService)
        {
            this.emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(SendMessageModel model)
        {
            Email email = await emailService.ComposeEmail(model.ThreadId, model.MessageContent);
            emailService.SendMail(email);

            return Ok();
        }
        

        [HttpGet("fetch")]
        public async Task<List<MailThreadBasic>> FetchEmail()
        {
            return await emailService.ReceiveMail();
        }

        [HttpGet("threads")]
        public async Task<List<ThreadViewModel>> GetMailThreadsListPaged()
        {
            return await emailService.GetThreadsListPaged();
        }

        [HttpGet("threads/{threadId}")]
        public async Task<ThreadViewModel> GetThread([FromRoute] int threadId)
        {
            return await emailService.GetThread(threadId);
        }
    }
}