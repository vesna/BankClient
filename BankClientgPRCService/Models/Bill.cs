using System.ComponentModel.DataAnnotations;

namespace BankClientgPRCService.Models
{
    public class Bill
    {
        [Key] public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Value { get; set; }
        public Guid UserId { get; set; }
    }
}
