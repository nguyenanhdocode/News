using Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Category : BaseEntity
    {
        public string Slug { get; set; }
        public string Name { get; set; }
        public int OrderIndex { get; set; }
        public ICollection<Post> Posts { get; set;}
        public string CreatedUserId { get; set; }
        public AppUser CreatedUser { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsHidden { get; set; }
    }
}
