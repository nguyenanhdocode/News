using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.TwoFactorAuth
{
    public class TwoFactorAuthModel
    {
        public string Token { get; set; }
        public string UserId { get; set; }
        public string Code { get; set; }
    }
}
