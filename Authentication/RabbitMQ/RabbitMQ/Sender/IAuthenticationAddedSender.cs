namespace Shared.RabbitMQ
{
    public interface IAuthenticationAddedSender
    {
        void SendAuthentication(Guid userId,string firstName,string lastName,string Address,string education);
    }
}