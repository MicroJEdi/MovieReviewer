using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityMVCTest.Data;
using IdentityMVCTest.Models;
using Microsoft.AspNetCore.Mvc;

namespace IdentityMVCTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly MessageBoardAppDbContext _dbContext;

        public HomeController(MessageBoardAppDbContext messageBoardAppDbContext)
        {
            _dbContext = messageBoardAppDbContext;
        }

        public IActionResult Index()
        {
            return View(_dbContext.Blogs);
        }
    }
}