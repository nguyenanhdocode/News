using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public interface ITwoFactorAuthRepository : IBaseRepository<TwoFactorAuth>
    {
        Task<TwoFactorAuth?> Get(string token, string userId);
        bool IsExpires(TwoFactorAuth tfa);  
        bool Verify(TwoFactorAuth tfa, string code);
        void IncreaseFailedCount(TwoFactorAuth tfa);
        Task<TwoFactorAuth?> Get(string token, string userId, string code);
    }
}
