namespace ApiGateway.Models
{
    public class UserAuthenticationModel
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Education { get; set; }
    }

}