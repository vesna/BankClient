using BankClientgPRCService.Contexts;
using BankClientgPRCService.Protos;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;

namespace BankClientgPRCService.Services
{
    public class UserApiService : UserService.UserServiceBase
    {
        private readonly BankClientContext _context;
        public UserApiService( BankClientContext context)
        {
            _context = context;
        }

        public override async Task<UserReply> GetUser(GetUserRequest request, ServerCallContext context)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Phone == request.Phone);
            if (user == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "User not found"));
            }
            UserReply userReply = new UserReply() { Id = user.Id.ToString(), Name = user.Name, Phone = user.Phone };
            return await Task.FromResult(userReply);
        }
    }
}
