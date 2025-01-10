using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TopiCos.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TopiCos.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userEmail = await _userManager.FindByEmailAsync(model.Email);
                if (userEmail!=null)
                {
                    ModelState.AddModelError("", "Email already exists!");
                    return View(model);

                }
                // Copy data from RegisterViewModel to IdentityUser
                var user = new IdentityUser
                {
                    UserName = model.Email,
                    Email = model.Email
                };


                // Store user data in AspNetUsers database table
                var result = await _userManager.CreateAsync(user, model.Password);


                // If user is successfully created, sign-in the user using
                // this would not keep the session cookie. 
                // So. if the browser is closed, user will not be yet logged in.


                // SignInManager and redirect to index action of HomeController
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("index", "home");
                }


                // If there are any errors, add them to the ModelState object
                // which will be displayed by the validation summary tag helper
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> Logout()
        {


            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");


        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {

            if (ModelState.IsValid)
            {

                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password,
                                                            model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                else
                {
                    ModelState.AddModelError("", "Invalid login attempt");

                }






            }

            return View(model);

        }

    }
}
