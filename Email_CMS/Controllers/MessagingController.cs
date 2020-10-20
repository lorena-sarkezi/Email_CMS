using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS.Core.Services;
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

        [HttpGet("fetch")]
        public void FetchEmail()
        {
            emailService.ReceiveMail();
        }
    }
}