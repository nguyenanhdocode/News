using Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Common
{
    public class Alert
    {
        public Alert(string type, string message)
        {
            Type = type;
            Message = message;
        }

        public string Type { get; set; }
        public string Message { get; set; }

    }
}
