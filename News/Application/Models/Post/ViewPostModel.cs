using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Post
{
    public class ViewPostModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Description { get; set; }
        public string Slug { get; set; }
        public AppUser CreatedUser { get; set; }
        public DateTime CreatedOn { get; set; }
        public Asset CoverPhoto { get; set; }
        public Category Category { get; set; }
        public int ViewCount { get; set; }
    }
}
