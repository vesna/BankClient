namespace BankClientgPRCService.Services.Abstractions
{
    public interface ITokenService
    {
        public string GenerateToken(string phone, string roleName);
        public string GetRoleNameFromToken(string stream);
        public string GetPhoneFromToken(string stream);

    }
}
