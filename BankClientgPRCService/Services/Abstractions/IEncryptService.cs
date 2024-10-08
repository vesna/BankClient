namespace BankClientgPRCService.Services.Abstractions
{
    public interface IEncryptService
    {
        string GenerateSalt();
        string HashPassword(string password, string salt);
    }
}
