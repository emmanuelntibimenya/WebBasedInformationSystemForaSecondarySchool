namespace SchoolManagementSystem.Models
{
    public class UserSubject
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }
        public bool Attended { get; set; }
        public double Score { get; set; }
        public string LearnerProfile { get; set; }
    }
}
