using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IdentityMVCTest.Data;
using IdentityMVCTest.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace IdentityMVCTest.Controllers
{
    public class BlogController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly RoleManager<AppRole> roleManager;
        private readonly MessageBoardAppDbContext context;

        public BlogController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager, MessageBoardAppDbContext context)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
            this.context = context;
        }

        // GET: Blog
        public async Task<IActionResult> Index()
        {
            return View(await context.Blogs.ToListAsync());
        }

        // GET: Blog
        public async Task<IActionResult> Search(string searchString)
        {
            IEnumerable<Blog> blogs;
            if (searchString == null || searchString.Trim().Length < 1)
            {
                blogs = await  context.Blogs.ToListAsync();
                return View("/Views/Home/Index.cshtml", blogs);
            }
            blogs = await context.Blogs.Where(b => b.Name.Contains(searchString)).ToListAsync();
            return View("/Views/Home/Index.cshtml", blogs);
        }

        // GET: Blog/Details/5
        public IActionResult Details(int? id)
        {
            ViewBag.Blog = context.Blogs.Where(b => b.BlogId == id).First();
            ViewBag.Posts = context.Posts.Where(p => p.BlogId == id).Include(p => p.Comments).ToList();
            return View();
        }

        // GET: Blog/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Blog/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("BlogId,UserId,Name,LogoURL,Created")] Blog blog)
        {
            if (ModelState.IsValid)
            {
                AppUser u = await userManager.GetUserAsync(User);
                blog.Created = DateTime.Now;
                blog.UserId = u.Id;
                context.Add(blog);
                await context.SaveChangesAsync();
                return RedirectToAction("Index","Home");
            }
            return View(blog);
        }

        // GET: Blog/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await context.Blogs.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }
            return View(blog);
        }

        // POST: Blog/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("BlogId,UserId,Name,LogoURL,Created")] Blog blog)
        {
            if (id != blog.BlogId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    context.Update(blog);
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogExists(blog.BlogId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(blog);
        }

        // GET: Blog/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await context.Blogs
                .FirstOrDefaultAsync(m => m.BlogId == id);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        // POST: Blog/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blog = await context.Blogs.FindAsync(id);
            context.Blogs.Remove(blog);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlogExists(int id)
        {
            return context.Blogs.Any(e => e.BlogId == id);
        }
    }
}
