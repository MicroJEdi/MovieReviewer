using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMVCTest.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        public string UserId { get; set; }
        public string ContentBody { get; set; }
        public DateTime Created { get; set; }

        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}
