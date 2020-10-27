using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMVCTest.Models
{
    public class Post
    {
        public int PostId { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public string ContentBody { get; set; }
        public DateTime Created { get; set; }

        public int BlogId { get; set; }
        public Blog Blog { get; set; }

        public List<Comment> Comments { get; set; }
    }
}
