using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using ChatApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace ChatApp.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<ChatUser> _userManager;
        private SignInManager<ChatUser> _signInManager;

        public AccountController(UserManager<ChatUser> UserManager, SignInManager<ChatUser> SignInManager)
        {
            _userManager = UserManager;
            _signInManager = SignInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel NewAccount)
        {
            if (!ModelState.IsValid)
                return View();

            ChatUser newUser = new ChatUser();
            newUser.UserName = NewAccount.Username;
            IdentityResult result = await _userManager.CreateAsync(newUser, NewAccount.Password);

            if (result.Succeeded)
                await _signInManager.SignInAsync(newUser, true);

            return RedirectToAction("Index","Home");
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel Login)
        {
            ChatUser user = await _userManager.FindByNameAsync(Login.Username);
            if(user == null)
            {
                ViewData["ErrorMessage"] = "Login failed";
                return View();
            }
            SignInResult sResult = await _signInManager.CheckPasswordSignInAsync(user, Login.Password, false);
            if(sResult.Succeeded == false)
            {
                ViewData["ErrorMessage"] = "Login failed";
                return View();
            }

            await _signInManager.SignInAsync(user, true);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}