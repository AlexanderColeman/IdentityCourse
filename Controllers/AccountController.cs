using IdentityCourse.Models;
using IdentityCourse.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityCourse.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManger;
        private readonly SignInManager<IdentityUser> _signInManger;

        public AccountController(UserManager<IdentityUser> userManger,SignInManager<IdentityUser> signInManger)
        {
            _userManger = userManger;
            _signInManger = signInManger;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            LoginViewModel loginViewModel = new LoginViewModel();
            loginViewModel.ReturnUrl = returnUrl ?? Url.Content("~/");
            return View(loginViewModel);
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManger.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password, loginViewModel.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid Login attempt");
                    return View(loginViewModel);
                }
            }
            return View(loginViewModel);
        }

        public async Task<IActionResult> Register(string? returnUrl = null)
        {
            RegisterViewModel registerViewModel = new RegisterViewModel();
            registerViewModel.ReturnUrl = returnUrl;
            return View(registerViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel, string? returnUrl = null)
        {
            
            registerViewModel.ReturnUrl = returnUrl;
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = new AppUser
                { 
                    Email = registerViewModel.Email, 
                    UserName = registerViewModel.UserName,
                };

                var result = await _userManger.CreateAsync(user, registerViewModel.Password);

                if (result.Succeeded)
                {
                    await _signInManger.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
                ModelState.AddModelError("Password", "User could not be created. password not unique enough");
            }
            return View(registerViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logoff
    }
}
