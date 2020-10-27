using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMVCTest.Models
{
    public class Blog
    {
        public int BlogId { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string LogoURL { get; set; }
        public DateTime Created { get; set; }

        public List<Post> Posts { get; set; }
    }
}
