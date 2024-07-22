using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedDate { get; set; }
        public ICollection<Asset> Assets { get; set; }
        public Guid? AvatarId { get; set; }
        public Asset Avatar { get; set; }
        public bool IsDisabled { get; set; }
        public ICollection<Post> Posts { get; set; }
        public ICollection<Post> UpdatedPosts { get; set; }
        public ICollection<Category> Categories { get; set; }
        public ICollection<TwoFactorAuth> TwoFactorAuths { get; set; }
    }
}
