using User.Domain.Models;

namespace User.BusinessLogic.Repositories
{
    public interface IUserRepository
    {
        Task<IList<UserEntity>> Get();
        Task<UserEntity> GetById(Guid userId);
        Task<Guid> Add(UserEntity user);
        Task Update(UserEntity userUser);
    }
}
