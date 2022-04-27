using Authentication.Domain.Models;

namespace Authentication.BusinessLogic.Repositories
{
    public interface IAuthenticationRepository
    {
        Task<IList<UserAuthentication>> Get();
        Task<UserAuthentication> GetById(Guid userId);
        Task<Guid> Add(UserAuthentication userAuthentication);
        Task<UserAuthentication> GetByMobileNumber(string mobileNumber);
        Task Update(UserAuthentication userAuthentication);
    }
}
