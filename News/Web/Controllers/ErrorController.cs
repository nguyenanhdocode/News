using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("Error")]
    public class ErrorController : Controller
    {

        [HttpGet]
        [Route("404")]
        public IActionResult PageNotFound()
        {
            return View();
        }

        [HttpGet]
        [Route("403")]
        public IActionResult Forbidden()
        {
            return View();
        }

        [HttpGet]
        [Route("422")]
        public IActionResult UnprocessableEntity()
        {
            return View();
        }

        [HttpGet]
        [Route("500")]
        public IActionResult InternalServerError()
        {
            return View();
        }
    }
}
