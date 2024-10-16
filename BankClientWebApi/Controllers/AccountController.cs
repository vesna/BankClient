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
    public class AccountController : ControllerBase
    {
        private readonly AccountService.AccountServiceClient? _client;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(IMapper mapper, ITokenService tokenService, IConfiguration config)
        {
            _client = new AccountService.AccountServiceClient(GrpcChannel.ForAddress(config.GetSection("Grpc:Channel").Get<string>()));
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("accounts")]
        [Authorize]
        [EnableCors("DefaultOrigins")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> ListAccountsAsync()
        {
            try
            {
                var q = HttpContext.Request.Headers.Authorization;
                var token = q[0].Split(' ')[1];
                var phone = _tokenService.GetPhoneFromToken(token);
                var acc = await _client.ListAccountsAsync(new AccountByUserRequest { Phone = phone});
                var response = _mapper.Map<ListAccountsResponse>(acc);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
