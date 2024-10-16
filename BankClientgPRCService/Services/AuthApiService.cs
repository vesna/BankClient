using BankClientgPRCService.Contexts;
using BankClientgPRCService.Models;
using BankClientgPRCService.Protos;
using BankClientgPRCService.Services.Abstractions;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;

namespace BankClientgPRCService.Services
{
    public class AuthApiService : AuthService.AuthServiceBase
    {
        private readonly IEncryptService _encryptService;
        private readonly BankClientContext _context;
        public AuthApiService(IEncryptService encryptService, BankClientContext context)
        {
            _encryptService = encryptService;
            _context = context;
        }

        public override async Task<AuthReply> Login(LoginRequest request, ServerCallContext context)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Phone == request.Phone);
            if (user == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "User not found"));
            }
            var pass = _encryptService.HashPassword(request.Password, user.Salt);
            if (!user.PasswordHash.SequenceEqual(pass))
            {
                throw new RpcException(new Status(StatusCode.Unauthenticated, "Unauthenticated"));
            }
            var role = await _context.Roles.FirstOrDefaultAsync(x => x.Id == user.RoleId);
          
            var reply = new AuthReply() { Id = user.Id.ToString(), Phone = user.Phone, RoleName = role.Name };
            return await Task.FromResult(reply);
        }

        public override async Task<AuthReply> Register(RegistrationRequest request, ServerCallContext context)
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
                PasswordHash = _encryptService.HashPassword(request.Password, salt),
                RoleId = role!.Id
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
          
            var reply = new AuthReply() { Id = user.Id.ToString(), Phone = user.Phone, RoleName = role.Name };
            return await Task.FromResult(reply);
        }
    }
}
