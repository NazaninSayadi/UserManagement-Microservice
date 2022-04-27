using System.ComponentModel.DataAnnotations;

namespace User.Domain.Models
{
    public class UserEntity
    {
        [Key]
        public Guid UserId { get; init; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Education { get; set; }
        public string? Address { get; set; }

    }
}
