namespace RabbitMQ
{
    public interface IUserAddedSender
    {
        void SendUser(Guid userId);
    }
}