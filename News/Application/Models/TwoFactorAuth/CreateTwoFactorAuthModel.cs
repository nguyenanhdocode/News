using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.TwoFactorAuth
{
    public class CreateTwoFactorAuthModel
    {
        public string UserId { get; set; }
        public string Token { get; set; }
        public string Code { get; set; }
        public DateTime Expires { get; set; }
    }
}
