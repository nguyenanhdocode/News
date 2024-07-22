using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Impl
{
    public class TwoFactorAuthRepository : BaseRepository<TwoFactorAuth>, ITwoFactorAuthRepository
    {
        public async Task<TwoFactorAuth?> Get(string token, string userId)
        {
            return await Dbcontext.TwoFactorAuths
                .SingleOrDefaultAsync(p => p.Token.Equals(token)
                    && p.UserId.Equals(userId));
        }

        public async Task<TwoFactorAuth?> Get(string token, string userId, string code)
        {
            return await Dbcontext.TwoFactorAuths
                .SingleOrDefaultAsync(p => p.Token.Equals(token)
                    && p.UserId.Equals(userId)
                    && p.Code.Equals(code));
        }

        public void IncreaseFailedCount(TwoFactorAuth tfa)
        {
            tfa.FailedCount += 1;
            Dbcontext.TwoFactorAuths.Update(tfa);
        }

        public bool IsExpires(TwoFactorAuth tfa)
        {
            return tfa.IsAuthenticated || DateTime.Compare(tfa.Expires, DateTime.UtcNow) <= 0;
        }

        public bool Verify(TwoFactorAuth tfa, string code)
        {
            return !IsExpires(tfa) && tfa.Code.Equals(code);
        }
    }
}
