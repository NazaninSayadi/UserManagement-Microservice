using Authentication.BusinessLogic.Models;
using Authentication.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.BusinessLogic.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<IEnumerable<UserAuthentication>> GetAll();
        Task<UserAuthentication> GetByUserId(Guid userId);
        Task<Guid> Register(string email, string mobileNumber);
        Task Update(UserAuthentication userAuthentication);

    }
}
