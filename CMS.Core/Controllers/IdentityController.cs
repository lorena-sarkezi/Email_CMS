﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS.Core.Services;
using CMS.Data.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Core.Controllers
{
    [Route("api/v1/identity")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IIdentityService identityService;

        public IdentityController(IIdentityService identityService)
        {
            this.identityService = identityService;
        }

        
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(RegisterModel model)
        {
            RegistrationStatusResponse status = await identityService.RegisterUser(model);
            if (status == RegistrationStatusResponse.SUCCESS) return Ok();

            return BadRequest(status);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser(LoginModel model)
        {
            string token = await identityService.LoginUser(model);
            if (token != null) return Ok(token);

            return Unauthorized();
        }

        [HttpGet("validate")]
        public IActionResult ValidateToken()
        {
            string authHeder = HttpContext.Request.Headers["Authorization"];

            if (authHeder == null) return Ok(false);

            string token = authHeder.Split(" ")[1];
            bool isTokenValid = identityService.ValidateToken(token);

            return Ok(isTokenValid);
        }
    }
}
