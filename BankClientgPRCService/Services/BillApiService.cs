using BankClientgPRCService.Contexts;
using BankClientgPRCService.Protos;
using BankClientgPRCService.Services.Abstractions;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;

namespace BankClientgPRCService.Services
{
    public class BillApiService : BillService.BillServiceBase
    {
        private readonly BankClientContext _context;
        private readonly ITokenService _tokenService;
        public BillApiService(ITokenService tokenService, BankClientContext context)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public override async Task<ListReply> ListBills(BillByUserRequest request, ServerCallContext context)
        {
            var listReply = new ListReply();
            var phone = _tokenService.GetPhoneFromToken(request.Token);
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Phone == phone);
            var billList = _context.Bills.Select(item => new BillReply { Name = item.Name, Value = item.Value, UserId = item.UserId.ToString() })
                .Where(item => item.UserId == user!.Id.ToString())
                .ToList();
            listReply.Bills.AddRange(billList);
            return await Task.FromResult(listReply);
        }
    }
}
