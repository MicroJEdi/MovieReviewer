using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityMVCTest.Data;
using IdentityMVCTest.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityMVCTest.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly RoleManager<AppRole> roleManager;
        private readonly MessageBoardAppDbContext context;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager, MessageBoardAppDbContext context)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
            this.context = context;
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Settings()
        {
            AppUser loggedInUser = await userManager.GetUserAsync(User);
            ViewBag.Blogs = context.Blogs.Where(b => b.UserId == loggedInUser.Id).ToList();
            ViewBag.Posts = context.Posts.Where(p => p.UserId == loggedInUser.Id).ToList();
            ViewBag.Comments = context.Comments.Where(c => c.UserId == loggedInUser.Id).ToList();
            return View();
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(string username, string email, string passwordhash)
        {
            AppUser user = new AppUser() { UserName = username, Email = email };
            IdentityResult result = await userManager.CreateAsync(user, passwordhash);
            if (result.Succeeded)
            {
                var signInResult = await signInManager.PasswordSignInAsync(username, passwordhash, true, lockoutOnFailure: true);
                user = await userManager.FindByEmailAsync(email);
                AppRole applicationRole = await roleManager.FindByNameAsync("User");
                if (applicationRole != null)
                {   
                    IdentityResult roleResult = await userManager.AddToRoleAsync(user, applicationRole.Name);
                    if (roleResult.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            
            return View();
        }

        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(string username, string passwordhash)
        {
            var result = await signInManager.PasswordSignInAsync(username, passwordhash, true, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
            
        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}