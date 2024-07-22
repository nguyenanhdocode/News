using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.User
{
    public class LockUserModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public bool IsDisabled { get; set; }
    }
}
