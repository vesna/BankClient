using System.ComponentModel.DataAnnotations;

namespace BankClientgPRCService.Models
{
    public class Role
    {
        [Key] public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
    }
}
