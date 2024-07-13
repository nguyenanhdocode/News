using Application.Common;
using Application.Exceptions;
using Application.Models.Common;
using Application.Models.User;
using Application.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("/Users")]
    public class UserController : Controller
    {
        private IUserService _userService;
        private IReCaptchaService _reCaptchaService;

        public UserController(IUserService userService
            , IReCaptchaService reCaptchaService)
        {
            _userService = userService;
            _reCaptchaService = reCaptchaService;
        }

        [HttpGet]
        [Route("Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([CustomizeValidator(Skip = true)] LoginModel model
            , [FromServices] IValidator<LoginModel> validator)
        {
            bool useReCaptcha = false;
            
            int accessFailedCount = await _userService.GetAccessFailedCount(model.Email ?? "");
            if (accessFailedCount >= 3)
            {
                useReCaptcha = true;
                ViewData["UseReCaptcha"] = useReCaptcha;
            }

            if (useReCaptcha)
            {
                string reCaptcha = Request.Form["g-Recaptcha-Response"];
                if (!_reCaptchaService.Validate(reCaptcha))
                {
                    ViewData["ReCaptchaError"] = "Vui lòng xác nhận rằng bạn không phải là người máy";
                    return View();
                }
            }
            else
            {
                var val = validator.Validate(model);
                if (!val.IsValid)
                {
                    val.AddToModelState(ModelState);
                    return View();
                }
            }

            try
            {
                var principal = await _userService.Login(model);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme
                    , principal);
                return Redirect("/");
            }
            catch (UnauthorizeException)
            {
                ViewData["error"] = "Thông tin đăng nhập không chính xác!";
            }

            //string reCaptcha = Request.Form["g-Recaptcha-Response"];
            //if (useReCaptcha && !_reCaptchaService.Validate(reCaptcha))
            //{
            //    ViewData["ReCaptchaError"] = "Vui lòng xác nhận rằng bạn không phải là người máy";
            //    return View();
            //}

            //if (!ModelState.IsValid)
            //    return View();

            //try
            //{
            //    var principal = await _userService.Login(model);
            //    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme
            //        , principal);
            //    return Redirect("/");
            //}
            //catch (UnauthorizeException)
            //{
            //    ViewData["error"] = "Thông tin đăng nhập không chính xác!";
            //}

            return View();
        }

        [HttpGet]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login", "User");
        }

        [HttpGet]
        [Route("ForgotPassword")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [Route("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(
            [CustomizeValidator(Skip = true)] ForgotPasswordModel model
            , [FromServices] IValidator<ForgotPasswordModel> validator)
        {
            string reCaptcha = Request.Form["g-Recaptcha-Response"];
            if (!_reCaptchaService.Validate(reCaptcha))
            {
                
                ViewData["ReCaptchaError"] = "Vui lòng xác nhận rằng bạn không phải là người máy";
                return View();
            }

            var val = validator.Validate(model);
            if (!val.IsValid)
            {
                val.AddToModelState(ModelState);
                return View();
            }

            await _userService.CreateForgotPasswordRequest(model);
            string message = string.Format("Chúng tôi vừa gửi link reset mật khẩu đến email của bạn.");
            ViewData["Alert"] = new Alert(AlertTypes.Success, message);
            return View();
        }

        [HttpGet]
        [Route("ResetPassword")]
        public IActionResult ResetPassword([FromQuery(Name = "Token")] string token
            , [FromQuery(Name = "UserId")] string userId)
        {
            ViewData["UserId"] = userId;
            ViewData["Token"] = token;
            return View();
        }

        [HttpPost]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                await _userService.ResetPassword(model);
                return RedirectToAction("Login", "User");
            }
            catch (UnprocessableEntityException)
            {
                string message = "Link reset mật khẩu không hợp lệ, vui lòng yêu cầu link mới.";
                ViewData["Alert"] = new Alert(AlertTypes.Danger, message);
            }

            return View();
        }
    }
}
