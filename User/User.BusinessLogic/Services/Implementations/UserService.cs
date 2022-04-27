using User.BusinessLogic.Services.Interfaces;
using User.BusinessLogic.Repositories;
using User.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.BusinessLogic.Models;

namespace User.BusinessLogic.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _UserRepository;
        public UserService(IUserRepository UserRepository)
        {
            _UserRepository = UserRepository;
        }
        public async Task<IEnumerable<UserEntity>> GetAll()
        {
            return await _UserRepository.Get();
        }

        public async Task<UserEntity> GetByUserId(Guid userId)
        {
            return await _UserRepository.GetById(userId);
        }

        public async Task<Guid> Register(Guid userId, string firstName, string lastName, string address, string education)
        {
            var user = await _UserRepository.GetById(userId);
            if (user != null)
                throw new Exception("user is repetitive");

            user = new UserEntity { FirstName = firstName,LastName = lastName, Address = address,Education = education , UserId = userId };
            return await _UserRepository.Add(user);
        }
        public async Task<Guid> AddUserId(Guid userId)
        {
            var user = await _UserRepository.GetById(userId);
            if (user != null)
                throw new Exception("user is repetitive");

            user = new UserEntity {UserId = userId };
            return await _UserRepository.Add(user);
        }
        public async Task Update(UserEntity userUser)
        {
            await _UserRepository.Update(userUser);

        }
    }
}
