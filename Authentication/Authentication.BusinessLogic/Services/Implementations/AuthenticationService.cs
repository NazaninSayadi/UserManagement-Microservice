using Authentication.BusinessLogic.Services.Interfaces;
using Authentication.BusinessLogic.Repositories;
using Authentication.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Authentication.BusinessLogic.Models;

namespace Authentication.BusinessLogic.Services.Implementations
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IAuthenticationRepository _authenticationRepository;
        public AuthenticationService(IAuthenticationRepository authenticationRepository)
        {
            _authenticationRepository = authenticationRepository;
        }
        public async Task<IEnumerable<UserAuthentication>> GetAll()
        {
            return await _authenticationRepository.Get();
        }

        public async Task<UserAuthentication> GetByUserId(Guid userId)
        {
            return await _authenticationRepository.GetById(userId);
        }

        public async Task<Guid> Register(string email, string mobileNumber)
        {
            var userAuthentication = await _authenticationRepository.GetByMobileNumber(mobileNumber);
            if (userAuthentication != null)
                throw new Exception("Auth is repetitive");

            userAuthentication = new UserAuthentication { Email = email, MobileNumber = mobileNumber, Status = Status.Pending };
            return await _authenticationRepository.Add(userAuthentication);
        }
        public async Task Update(UserAuthentication userAuthentication)
        {
            await _authenticationRepository.Update(userAuthentication);

        }
    }
}
