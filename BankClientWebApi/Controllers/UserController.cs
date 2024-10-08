using BankClientWebApi.Exceptions;
using BankClientWebApi.Filters;
using BankClientWebApi.Protos;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BankClientWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly UserService.UserServiceClient? _client;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
            _client = new UserService.UserServiceClient(GrpcChannel.ForAddress("https://localhost:7052"));
        }

        [HttpPost]
        [Route("register")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ProducesResponseType(typeof(CreateUserRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
        public ActionResult<string> Register([FromBody, Required] CreateUserRequest request)
        {
            try
            {
                var token = _client.RegisterUser(request);
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
        [ProducesResponseType(typeof(LoginUserRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
        public ActionResult<string> Login([FromBody, Required] LoginUserRequest request)
        {
            try
            {
                var token = _client.Login(request);
                return Ok(token);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getUser/{token}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
        public ActionResult<UserReply> GetUser([FromRoute, Required] string token)
        {
            try
            {
                var reply = _client.GetUser(new GetUserRequest { Token = token});
                return Ok(reply);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
