namespace User.BusinessLogic.Models
{
    public class UserCreateModel
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Education { get; set; }
        public string Address { get; set; }
    }
}
