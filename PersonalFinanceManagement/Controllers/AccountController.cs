using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PersonalFinanceManagement.Models;
using PersonalFinanceManagement.ViewModels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace PersonalFinanceManagement.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            if (string.IsNullOrEmpty(returnUrl))
            {
                returnUrl = "/";
            }
            ViewData["ReturnUrl"] = returnUrl;

            // Check if there's a message stored in TempData
            if (TempData.ContainsKey("Message"))
            {
                ViewBag.Message = TempData["Message"].ToString();
            }

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (string.IsNullOrEmpty(returnUrl))
            {
                returnUrl = Url.Content("~/");
            }
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
        
                if (result.Succeeded)
                {if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid login attempt.");
                }
            }
        
            // Model is invalid or login failed. Return the view with errors.
            return View(model);
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
        
            return View();
        }
        
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User { UserName = model.Name, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
        
                if (result.Succeeded)
                {
                    // If registration succeeded, sign the user in and redirect them to the home page.
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
        
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
        
            // Model is invalid or registration failed. Return the view with errors.
            return View(model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            TempData["Message"] = "You have been logged out successfully.";
            return RedirectToAction("Login", "Account");
        }
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            // Load the user profile view here

            return View();
        }
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
        
            // Return a RedirectToAction response instead of a Redirect response for null or non-local URLs
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

    }
}
