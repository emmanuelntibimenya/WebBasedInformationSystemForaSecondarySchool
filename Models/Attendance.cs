namespace SchoolManagementSystem.Models
{
    public class Attendance
    {
        public int Id { get; set; }
        public User Student { get; set; }
        public Subject Program { get; set; }

        public Attendance()
        {
            Student = new User();
            Program = new Subject();
        }
    }
}
