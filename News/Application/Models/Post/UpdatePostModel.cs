using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Post
{
    public class UpdatePostModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public bool IsPinned { get; set; }
        public bool IsDraft { get; set; }
        public string CategoryId { get; set; }
        public IFormFile CoverPhoto { get; set; }
    }
}
