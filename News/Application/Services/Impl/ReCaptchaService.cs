using Application.Common;
using Application.Models.ReCaptcha;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Impl
{
    public class ReCaptchaService : IReCaptchaService
    {
        private ReCaptchaConfiguration _configuration;

        public ReCaptchaService(IOptions<ReCaptchaConfiguration> configuration)
        {
            _configuration = configuration.Value;
        }

        public bool Validate(string token)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrWhiteSpace(token))
                return false;

            var client = new System.Net.WebClient();

            string secretKey = _configuration.Secret;

            var GoogleReply = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secretKey, token));

            var captchaResponse = Newtonsoft.Json.JsonConvert
                .DeserializeObject<ReCaptchaReponseModel>(GoogleReply);

            return captchaResponse.Success.ToLower().Equals("true");
        }
    }
}
