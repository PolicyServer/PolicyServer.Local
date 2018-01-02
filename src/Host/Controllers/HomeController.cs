using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Host.Models;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using PolicyServer.Client;

namespace Host.Controllers
{
    public class HomeController : Controller
    {
        private readonly PolicyServerClient _policy;

        public HomeController(PolicyServerClient policy)
        {
            _policy = policy;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        //[Authorize(Roles="doctor,nurse")]
        //[Authorize("SeePatients")]
        public async Task<IActionResult> Secure()
        {
            var result = await _policy.EvaluateAsync(User);

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
