using BeFitApp.Data;
using BeFitApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BeFitApp.Controllers
{
    [Route("AppUser/[action]")]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;

        public UserController(UserManager<AppUser> userManager, AppDbContext context)
        {
            this._userManager = userManager;
            this._context = context;
        }
        [HttpGet]
        public async Task<IActionResult> SaveUserInfo()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveUserInfo(AppUser model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            user.FirstName = model.FirstName;
            user.Age = model.Age;
            user.Weight = model.Weight;
            user.Height = model.Height;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                TempData["Message"] = "User information updated successfully.";
                return RedirectToAction("Index", "Home");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
