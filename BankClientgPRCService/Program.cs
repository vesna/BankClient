using BankClientgPRCService.Contexts;
using BankClientgPRCService.Securities;
using BankClientgPRCService.Services;
using BankClientgPRCService.Services.Abstractions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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

            var jwt = builder.Configuration.GetSection("JwtConfiguration").Get<JwtConfiguration>()
               ?? throw new Exception("JwtConfiguration not found");
            builder.Services
                .AddSingleton(provider => jwt)
                .AddSingleton<IEncryptService, EncryptService>()
                .AddSingleton<ITokenService, TokenService>();


            builder.Services.AddAuthorization();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwt.Issuer,
                        ValidAudience = jwt.Audience,
                        IssuerSigningKey = jwt.GetSingingKey()
                    };

                });

            var app = builder.Build();
            app.MapGrpcService<UserApiService>();
            app.MapGrpcService<BillApiService>();

            app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client...");
            app.Run();
        }
    }
}
