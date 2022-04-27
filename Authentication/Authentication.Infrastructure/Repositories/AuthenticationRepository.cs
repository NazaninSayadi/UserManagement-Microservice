using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Authentication.BusinessLogic.Repositories;
using Authentication.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Infrastructure.Repositories
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly AuthrnticationContext _context;
        public AuthenticationRepository(AuthrnticationContext context)
        {
            _context = context;
        }
        public async Task<Guid> Add(UserAuthentication userAuthentication)
        {
            await _context.UserAuthentications.AddAsync(userAuthentication);
            await _context.SaveChangesAsync();
            return userAuthentication.UserId;
        }

        public async Task<IList<UserAuthentication>> Get()
        {
            return await _context.UserAuthentications.AsNoTracking().ToListAsync();
        }

        public async Task<UserAuthentication> GetById(Guid userId)
        {
            return await _context.UserAuthentications.FindAsync(userId);
        }

        public async Task<UserAuthentication> GetByMobileNumber(string mobileNumber)
        {
            return await _context.UserAuthentications.Where(x=>x.MobileNumber == mobileNumber).SingleOrDefaultAsync();
        }

        public async Task Update(UserAuthentication userAuthentication)
        {
            _context.UserAuthentications.Update(userAuthentication);
            await _context.SaveChangesAsync();
        }
    }
}
