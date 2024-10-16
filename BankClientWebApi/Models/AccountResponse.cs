namespace BankClientWebApi.Models
{
    public class AccountResponse
    {
        public string Name { get; set; }
        public string Value {  get; set; }
    }

    public class ListAccountsResponse
    {
        public List<AccountResponse> Accounts { get; set; }
    }
}
