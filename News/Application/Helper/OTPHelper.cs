using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helper
{
    public class OTPHelper
    {
        private static readonly string chars = "0123456789";

        public static string GenerateOTP()
        {
            var random = new Random();
            var result = new string(Enumerable.Repeat(chars, 6).Select(s => s[random.Next(s.Length)]).ToArray());

            return result.ToString();
        }
    }
}
