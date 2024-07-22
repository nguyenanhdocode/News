using Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Post : BaseEntity, IAuditedEntity
    {
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public int ViewCount { get; set; }
        public string CreatedUserId { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? UpdatedUserId { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public AppUser CreatedUser { get; set; }
        public AppUser UpdatedUser { get;set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
        public bool IsPinned { get; set; }
        public bool IsDraft { get; set; }
        public Guid CoverPhotoId { get; set; }
        public Asset CoverPhoto { get; set; }
    }
}
