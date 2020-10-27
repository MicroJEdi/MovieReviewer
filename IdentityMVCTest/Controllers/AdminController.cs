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
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly RoleManager<AppRole> roleManager;
        private readonly MessageBoardAppDbContext context;

        public AdminController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager, MessageBoardAppDbContext context)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
            this.context = context;
        }

        public IActionResult Index()
        {
            ViewBag.Blogs = context.Blogs.ToList();
            ViewBag.Posts = context.Posts.ToList();
            ViewBag.Comments = context.Comments.ToList();
            return View();
        }
    }
}