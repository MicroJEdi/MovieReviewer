using IdentityMVCTest.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMVCTest.Data
{
    public class MessageBoardAppDbContext : DbContext
    {
        public MessageBoardAppDbContext(DbContextOptions<MessageBoardAppDbContext> options) : base(options)
        {

        }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}
