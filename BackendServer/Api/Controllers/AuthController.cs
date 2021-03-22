using Api.Interfaces;
using Api.Responses;
using Api.Services.IServices;
using AutoMapper;
using Core.CustomEntities;
using Core.Dtos;
using Core.Entities;
using Core.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService service;
        private readonly IMapper mapper;
        private readonly ITokenProvider provider;
        private readonly IEmailSender emailSender;

        public AuthController(IAuthService service, IMapper mapper, ITokenProvider provider, IEmailSender emailSender )
        {
            this.service = service;
            this.mapper = mapper;
            this.provider = provider;
            this.emailSender = emailSender;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserCredentials credentials)
        {
            var user = await this.service.Login(credentials);

            if (user != null)
            {
                var token = provider.Token(user);
                var response = new ApiResponse<string>(token);
                return Ok(response);
            }
            else 
            {
                return BadRequest("No user found");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserDto userDto)
        {
            var domain = this.mapper.Map<User>(userDto);
            var user = await this.service.Register(domain);

            if (user != null)
            {
                var token = provider.Token(user);
                var response = new ApiResponse<string>(token);
                return Ok(response);
            }
            else
            {
                return BadRequest("No user found");
            }
        }

        [HttpGet]
        public async Task<IActionResult> RecoverPassword([FromQuery] string email, string newPassword)
        {
            var user = await this.service.RecoverPassword(email, newPassword);

            if (user)
            {
                await this.emailSender.SenderEmail(email,"Password recovering", $"Your new password is { newPassword }");
                var response = new ApiResponse<bool>(user);
                return Ok(response);
            }
            else
            {
                var response = new ApiResponse<bool>(user);
                return BadRequest(response);
            }
        }
    }
}
