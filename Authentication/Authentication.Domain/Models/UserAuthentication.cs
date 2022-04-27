using System.ComponentModel.DataAnnotations;

namespace Authentication.Domain.Models
{
    public class UserAuthentication
    {
        public UserAuthentication()
        {
            UserId = new Guid();
        }
        [Key]
        public Guid UserId { get; private set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public Status  Status { get; set; }
    }
    public enum Status
    {
        Pending,
        Approved
    }
}
