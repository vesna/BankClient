using BankClientgPRCService.Contexts;
using BankClientgPRCService.Protos;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;

namespace BankClientgPRCService.Services
{
    public class AccountApiService : AccountService.AccountServiceBase
    {
        private readonly BankClientContext _context;
        public AccountApiService(BankClientContext context)
        {
            _context = context;
        }

        public override async Task<ListReply> ListAccounts(AccountByUserRequest request, ServerCallContext context)
        {
            var listReply = new ListReply();
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Phone == request.Phone);
            if (user == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "User not found"));
            }
            var accountList = _context.Accounts.Select(item => new AccountReply { Name = item.Name, Value = item.Value, UserId = item.UserId.ToString() })
                .Where(item => item.UserId == user!.Id.ToString())
                .ToList();
            listReply.Accounts.AddRange(accountList);
            return await Task.FromResult(listReply);
        }
    }
}
