using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators.Post
{
    public class PostValidatorConfiguration
    {
        public static readonly List<string> AcceptImageFormats = new List<string> 
        {
            ".jpg", ".png", ".jpeg"
        };
    }
}
