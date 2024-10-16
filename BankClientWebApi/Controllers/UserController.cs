using AutoMapper;
using BankClientWebApi.Exceptions;
using BankClientWebApi.Models;
using BankClientWebApi.Protos;
using BankClientWebApi.Services.Abstractions;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace BankClientWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService.UserServiceClient? _client;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public UserController(IMapper mapper, ITokenService tokenService, IConfiguration config)
        {
            _client = new UserService.UserServiceClient(GrpcChannel.ForAddress(config.GetSection("Grpc:Channel").Get<string>()));
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("info")]
        [Authorize]
        [EnableCors("DefaultOrigins")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetUserAsync()
        {
            try
            {
              var q = HttpContext.Request.Headers.Authorization;
                var token = q[0].Split(' ')[1];
                var phone = _tokenService.GetPhoneFromToken(token);

                var reply = await _client.GetUserAsync(new GetUserRequest { Phone = phone });

                var response = _mapper.Map<UserResponse>(reply);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
