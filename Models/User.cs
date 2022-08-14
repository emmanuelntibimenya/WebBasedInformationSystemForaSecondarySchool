using Microsoft.AspNetCore.Identity;

namespace SchoolManagementSystem.Models
{
    public class User : IdentityUser
    {
        public string? FullName { get; set; }
        
        public List<UserSubject>? Subjects { get; set; }
        public string? ParentId { get; set; }
        public User? Parent { get; set; }
        public List<User> Children { get; set; }

        public User()
        {
            Children = new List<User>();
        }
    }
}
