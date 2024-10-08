using BankClientgPRCService.Contexts;
using BankClientgPRCService.Models;
using BankClientgPRCService.Protos;
using BankClientgPRCService.Services.Abstractions;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace BankClientgPRCService.Services
{
    public class UserApiService : UserService.UserServiceBase
    {
        private readonly ITokenService _tokenService;
        private readonly IEncryptService _encryptService;
        private readonly BankClientContext _context;
        public UserApiService(ITokenService tokenService, IEncryptService encryptService, BankClientContext context)
        {
            _tokenService = tokenService;
            _encryptService = encryptService;
            _context = context;
        }

        public override async Task<TokenReply> Login(LoginUserRequest request, ServerCallContext context)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Phone == request.Phone);
            if (user == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "User not found"));
            }
            var pass = _encryptService.HashPassword(request.Password, user.Salt);
            if (!user.Password.SequenceEqual(pass))
            {
                throw new RpcException(new Status(StatusCode.Unauthenticated, "Unauthenticated"));
            }
            var role = await _context.Roles.FirstOrDefaultAsync(x => x.Id == user.RoleId);
            var token = _tokenService.GenerateToken(user.Phone, role!.Name.ToString());
            var reply = new TokenReply() { Token = token };
            return await Task.FromResult(reply);
        }

        public override async Task<UserReply> GetUser(GetUserRequest request, ServerCallContext context)
        {
            var phone = _tokenService.GetPhoneFromToken(request.Token);
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Phone == phone);
            if (user == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "User not found"));
            }
            var role = await _context.Roles.FindAsync(user.RoleId);
            UserReply userReply = new UserReply() { Name = user.Name, Phone = user.Phone };
            return await Task.FromResult(userReply);
        }

        public override async Task<TokenReply> RegisterUser(CreateUserRequest request, ServerCallContext context)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Phone == request.Phone);
            if (user is not null)
            {
                throw new RpcException(new Status(StatusCode.AlreadyExists, "User already exists"));
            }

            var role = await _context.Roles.FirstOrDefaultAsync(x => x.Name == request.RoleName.ToString());
            if (role is null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Role {request.RoleName} not found. Please use 'Admin' or 'User' role."));
            }
            var salt = _encryptService.GenerateSalt();
            user = new User
            {
                Name = request.Name,
                Phone = request.Phone,
                Salt = salt,
                Password = _encryptService.HashPassword(request.Password, salt),
                RoleId = role!.Id
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            var token = _tokenService.GenerateToken(user.Phone, role.Name.ToString());

            var reply = new TokenReply() { Token = token };
            return await Task.FromResult(reply);
        }

    }
}
