using Application.Common;
using Application.Exceptions;
using Application.Helper;
using Application.Models.Common;
using Application.Models.TwoFactorAuth;
using Application.Models.User;
using Application.Services;
using Core.Common;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using System.Security.Claims;

namespace Web.Controllers
{
    [Route("/Users")]
    public class UserController : Controller
    {
        private IUserService _userService;
        private IReCaptchaService _reCaptchaService;
        private IPostService _postService;
        private ICategoryService _categoryService;
        private IRoleService _roleService;
        private ITwoFactorAuthService _twoFactorAuthService;

        public UserController(IUserService userService
            , IReCaptchaService reCaptchaService
            , IPostService postService
            , ICategoryService categoryService
            , IRoleService roleService
            , ITwoFactorAuthService twoFactorAuthService)
        {
            _userService = userService;
            _reCaptchaService = reCaptchaService;
            _postService = postService;
            _categoryService = categoryService;
            _roleService = roleService;
            _twoFactorAuthService = twoFactorAuthService;
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

            string reCaptcha = Request.Form["g-Recaptcha-Response"];
            if (useReCaptcha && !_reCaptchaService.Validate(reCaptcha))
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

            try
            {
                var principal = await _userService.Login(model);

                var userId = principal.Claims
                    .SingleOrDefault(p => p.Type.Equals(ClaimTypes.NameIdentifier))?
                    .Value ?? string.Empty;

                if (await _userService.IsInRole(userId, Roles.Administrator) 
                    || await _userService.IsInRole(userId, Roles.Moderator))
                {
                    var tfa = new CreateTwoFactorAuthModel
                    {
                        UserId = userId,
                        Token = Guid.NewGuid().ToString(),
                        Code = OTPHelper.GenerateOTP(),
                        Expires = DateTime.UtcNow.AddHours(24),
                    };

                    await _twoFactorAuthService.Create(tfa);

                    var user = await _userService.GetProfile(userId);

                    await _twoFactorAuthService.SendCode(new SendCodeModel
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        Code = tfa.Code
                    });

                    string url = string.Format("/Users/TwoFactorAuth/{0}/{1}", tfa.Token, tfa.UserId);
                    return Redirect(url);
                }

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme
                    , principal);
                return Redirect("/");
            }
            catch (UnauthorizeException)
            {
                ViewData["error"] = "Thông tin đăng nhập không chính xác!";
            }
            catch (ForbiddenException)
            {
                ViewData["error"] = "Tài khoản của bạn đã bị khoá!";
            }

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

        [HttpGet]
        [Route("Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(
            [CustomizeValidator(Skip = true)] RegisterModel model
            , [FromServices] IValidator<RegisterModel> validator)
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

            try
            {
                await _userService.Register(model);
                model = new RegisterModel();
                string message = "Đăng ký thành công! Vui lòng kiểm tra hộp thư và xác nhận tài khoản.";
                ViewData["Alert"] = new Alert(AlertTypes.Success, message);
            }
            catch (UnprocessableEntityException)
            {
                string message = "Xảy ra lỗi trong quá trình tạo tài khoản.";
                ViewData["Alert"] = new Alert(AlertTypes.Danger, message);
            }

            return View();
        }

        [HttpGet]
        [Route("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string token
            , [FromQuery] string userId)
        {
            try
            {
                await _userService.ConfirmEmail(new ConfirmEmailModel
                {
                    Token = token,
                    UserId = userId
                });
                return RedirectToAction("Login", "Users");
            }
            catch (UnprocessableEntityException)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("Profile")]
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var profile = await _userService.GetProfile();

            return View(profile);
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize]
        public async Task<IActionResult> Profile(string id)
        {
            var profile = await _userService.GetProfile(id);
            var roles = await _roleService.GetRolesOfUser(id);

            ViewData["Roles"] = roles;

            return View(profile);
        }

        [HttpGet]
        [Authorize(Roles = Roles.Administrator + "," + Roles.Moderator)]
        public async Task<IActionResult> Index([FromQuery] Dictionary<string, string> param)
        {
            var users = await _userService.GetAll(param);

            return View(users);
        }

        [HttpGet]
        [Authorize(Roles = Roles.Administrator + "," + Roles.Moderator)]
        [Route("{userId}/Lock")]
        public async Task<IActionResult> Lock(string userId)
        {
            if (await _userService.IsInRole(userId, Roles.Administrator))
                throw new ForbiddenException();
            var profile = await _userService.GetProfile(userId);

            return View(new LockUserModel
            {
                Id = profile.Id,
                Email = profile.Email,
                IsDisabled = profile.IsDisabled
            });
        }

        [HttpGet]
        [Authorize(Roles = Roles.Administrator)]
        [Route("{userId}/Delete")]
        public async Task<IActionResult> Delete(string userId)
        {
            var profile = await _userService.GetProfile(userId);
            var posts = await _postService.GetPostsOfUser(userId);
            var cates = await _categoryService.GetCatesOfUser(userId);

            ViewData["Profile"] = profile;
            ViewData["Posts"] = posts;
            ViewData["Cates"] = cates;

            return View(new DeleteUserModel { Id = profile.Id });
        }

        [HttpPost]
        [Authorize(Roles = Roles.Administrator)]
        [Route("{userId}/Delete")]
        public async Task<IActionResult> Delete(DeleteUserModel model)
        {
            await _userService.Delete(model.Id);

            return RedirectToAction("Index", "User");
        }

        [HttpPost]
        [Authorize(Roles = Roles.Administrator + "," + Roles.Moderator)]
        [Route("{userId}/Lock")]
        public async Task<IActionResult> Lock(LockUserModel model)
        {
            await _userService.LockUser(model.Id);

            return RedirectToAction("Index", "User");
        }

        [HttpGet]
        [Authorize(Roles = Roles.Administrator)]
        [Route("{userId}/Roles/Update")]
        public async Task<IActionResult> UpdateRoles(string userId)
        {
            var userRoles = await _roleService.GetRolesOfUser(userId);
            var excludedRoles = new List<string>()
            {
                Roles.Administrator
            };
            excludedRoles.AddRange(userRoles);

            var roles = await _roleService.GetAll(excludedRoles);

            ViewData["Roles"] = roles;
            ViewData["UserRoles"] = userRoles;

            return View();
        }

        [HttpPost]
        [Authorize(Roles = Roles.Administrator)]
        [Route("{userId}/Roles/Update")]
        public async Task<IActionResult> UpdateRoles(UpdateRolesModel model)
        {
            await _userService.UpdateRoles(model);

            return RedirectToAction("Index", "User");
        }

        [HttpGet]
        [Route("TwoFactorAuth/{token}/{userId}")]
        public async Task<IActionResult> TwoFactorAuth(string token, string userId)
        {
            if (!await _twoFactorAuthService.Validate(token, userId))
                throw new NotFoundException("Invalid token");

            return View(new TwoFactorAuthModel
            {
                UserId = userId,
                Token = token
            });
        }

        [HttpPost]
        [Route("TwoFactorAuth/{token}/{userId}")]
        public async Task<IActionResult> TwoFactorAuth(TwoFactorAuthModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                var principal = await _twoFactorAuthService.Login(model);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme
                        , principal);
            }
            catch (UnauthorizeException)
            {
                ViewData["Error"] = "Mã xác thực không chính xác.";
                return View();
            }
            catch (ForbiddenException)
            {
                return RedirectToAction("Login", "User");
            }

            return Redirect("/");
        }
    }
}
