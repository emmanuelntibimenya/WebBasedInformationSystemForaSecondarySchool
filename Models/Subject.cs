namespace SchoolManagementSystem.Models
{
    public class Subject
    {
        public int Id { get; internal set; }
        public string? Title { get; set; }
        public List<User> Students { get; set; }

        public Subject()
        {
            Students = new List<User>();
        }
    }
}