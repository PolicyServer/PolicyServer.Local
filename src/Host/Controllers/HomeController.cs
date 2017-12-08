using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Host.Models;
using Microsoft.AspNetCore.Authorization;
using PolicyServerLocal;

namespace Host.Controllers
{
    public class HomeController : Controller
    {
        private readonly Policy _policy;

        public HomeController(Policy policy)
        {
            _policy = policy;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Secure()
        {
            var result = _policy.Evaluate(User);

            ViewData["roles"] = result.Roles;
            ViewData["perms"] = result.Permissions;

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
