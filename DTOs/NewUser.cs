using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.DTOs
{
    public class NewUser
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Password { get; set; }

        public NewUser()
        {
            FullName = string.Empty;
            Email = string.Empty;
            PhoneNumber = string.Empty;
            Password = string.Empty;
        }
    }
}
