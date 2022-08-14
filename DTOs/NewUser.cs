using SchoolManagementSystem.Constants;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.DTOs
{
    public class NewUser
    {
        public string? Id { get; set; }
        [Required]
        public string? FullName { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? PhoneNumber { get; set; }
        public string? Role { get; set; }
        public string Password { get; set; }
        public List<int> Subjects { get; set; }
        public string? Parent { get; set; }
        public NewUser()
        {
            Id = string.Empty;
            FullName = string.Empty;
            Email = string.Empty;
            PhoneNumber = string.Empty;
            Role = string.Empty;
            Password = string.Empty;
            Subjects = new List<int>();
        }
    }
}
