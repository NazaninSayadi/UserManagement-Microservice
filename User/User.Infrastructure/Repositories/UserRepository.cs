using User.BusinessLogic.Repositories;
using User.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace User.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserContext _context;
        public UserRepository(UserContext context)
        {
            _context = context;
        }
        public async Task<Guid> Add(UserEntity user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user.UserId;
        }

        public async Task<IList<UserEntity>> Get()
        {
            return await _context.Users.AsNoTracking().ToListAsync();
        }

        public async Task<UserEntity> GetById(Guid userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public Task Update(UserEntity userUser)
        {
            throw new NotImplementedException();
        }
    }
}
