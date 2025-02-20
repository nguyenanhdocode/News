﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common
{
    public class JwtConfiguration
    {
        public string SecretKey { get; set; }
    }

    public class SmtpConfiguration
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string Username { get; set;}
        public string Password { get; set; }
    }

    public class CloudinaryConfiguration
    {
        public string Url { get; set; }
        public string ViewImageUrl { get; set; }
    }

    public class ReCaptchaConfiguration
    {
        public string Key { get; set; }
        public string Secret { get; set; }
    }

    public class CryptographyConfiguration
    {
        public string Key { get; set; }
    }
}
