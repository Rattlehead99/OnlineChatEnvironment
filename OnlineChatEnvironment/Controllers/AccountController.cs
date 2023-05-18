using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineChatEnvironment.Data.Models;

namespace OnlineChatEnvironment.Controllers
{
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private SignInManager<User> signInManager;
        private UserManager<User> userManager;

        public AccountController(SignInManager<User> signInManager, UserManager<User> userManager)
        {
           this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string userName, string password)
        {
            var user = await  userManager.FindByNameAsync(userName);
            var result = await signInManager.PasswordSignInAsync(user, password, false, false);

            if (user == null)
            {
                return RedirectToAction("Login, Account");
            }

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Login", "Account");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string userName, string password)
        {
            var user = new User
            {
                UserName = userName
            };

            var result = await userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await signInManager.SignInAsync(user, false);

                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Register", "Account");
        }

        
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();

            return RedirectToAction("Login", "Account");
        }
    }
}
