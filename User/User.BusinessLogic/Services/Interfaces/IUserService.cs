using User.Domain.Models;

namespace User.BusinessLogic.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserEntity>> GetAll();
        Task<UserEntity> GetByUserId(Guid userId);
        Task<Guid> Register(Guid userId,string firstName, string lastName, string address, string education);
        Task<Guid> AddUserId(Guid userId);

        Task Update(UserEntity userUser);

    }
}
