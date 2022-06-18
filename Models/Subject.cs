namespace SchoolManagementSystem.Models
{
    public class Subject
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public Subject()
        {
            Title = string.Empty;
        }
    }
}
