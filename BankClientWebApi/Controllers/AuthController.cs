using BankClientWebApi.Exceptions;
using BankClientWebApi.Filters;
using BankClientWebApi.Protos;
using BankClientWebApi.Services.Abstractions;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BankClientWebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService.AuthServiceClient? _client;
        private readonly ITokenService _tokenService;

        public AuthController(ITokenService tokenService, IConfiguration config)
        {
            _client = new AuthService.AuthServiceClient(GrpcChannel.ForAddress(config.GetSection("Grpc:Channel").Get<string>()));
            _tokenService = tokenService;
        }

        [HttpPost]
        [Route("register")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ProducesResponseType(typeof(RegistrationRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> RegisterAsync([FromBody, Required] RegistrationRequest request)
        {
            try
            {
                var authReply = await _client.RegisterAsync(request);
                var token = _tokenService.GenerateToken(authReply.Phone, authReply.RoleName);
                return Ok(token);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("login")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ProducesResponseType(typeof(LoginRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> LoginAsync([FromBody, Required] LoginRequest request)
        {
            try
            {
                var authReply = await _client.LoginAsync(request);
                string token = _tokenService.GenerateToken(authReply.Phone, authReply.RoleName);
                return Ok(token);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
