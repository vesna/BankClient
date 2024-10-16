using BankClientgPRCService.Contexts;
using BankClientgPRCService.Services;
using BankClientgPRCService.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace BankClientgPRCService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<BankClientContext>(options =>
            {
                options.UseLazyLoadingProxies().UseNpgsql(builder.Configuration.GetConnectionString("BankClientdb"));
            });

            builder.Services.AddGrpc();
            builder.Services.AddSingleton<IEncryptService, EncryptService>();

            var app = builder.Build();

            app.MapGrpcService<UserApiService>();
            app.MapGrpcService<AccountApiService>();
            app.MapGrpcService<AuthApiService>();

            app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client...");
            app.Run();
        }
    }
}
