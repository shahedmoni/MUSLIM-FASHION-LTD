﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MuslimFashion.BusinessLogic;
using MuslimFashion.ViewModel;
using System.Threading.Tasks;

namespace MuslimFashion.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ICustomerCore _customer;
        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ICustomerCore customer)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _customer = customer;
        }

        //GET: Login
        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Dashboard");

            return View();
        }


        //POST: Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);

            if (result.Succeeded)
                return LocalRedirect(returnUrl ??= Url.Content("~/Dashboard/Index"));

            if (result.RequiresTwoFactor) return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, model.RememberMe });

            if (result.IsLockedOut)
                return RedirectToPage("./Lockout");


            ModelState.AddModelError(string.Empty, "Incorrect username or password");
            return View(model);
        }


        // GET: ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        // POST: ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(model);
            }

            await _signInManager.RefreshSignInAsync(user);

            return RedirectToAction("Index", "Dashboard");
        }

        //POST: logout
        [HttpPost]
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();

            if (returnUrl != null) return LocalRedirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }


        //POST: Customer Registration
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CustomerRegistration(CustomerAddModel model)
        {
            var response = await _customer.AddAsync(model);
            if (response.IsSuccess)
                await _signInManager.SignInAsync(response.Data, false);
            return Json(response);
        }
    }
}
