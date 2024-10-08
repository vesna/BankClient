using BankClientWebApi.Exceptions;
using BankClientWebApi.Protos;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BankClientWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BillController : ControllerBase
    {
        private readonly ILogger<BillController> _logger;
        private readonly BillService.BillServiceClient? _client;

        public BillController(ILogger<BillController> logger)
        {
            _logger = logger;
            _client = new BillService.BillServiceClient(GrpcChannel.ForAddress("https://localhost:7052"));
        }

        [HttpGet]
        [Route("getBills/{token}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
        public ActionResult<ListReply> GetBillsByUser([FromRoute, Required] string token)
        {
            try
            {
                ListReply bills = _client.ListBills(new BillByUserRequest { Token = token});
                return Ok(bills);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
