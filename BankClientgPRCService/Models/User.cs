using System.ComponentModel.DataAnnotations;

namespace BankClientgPRCService.Models
{
    public class User
    {
        [Key] public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Phone { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        public Guid RoleId { get; set; }
    }
}
