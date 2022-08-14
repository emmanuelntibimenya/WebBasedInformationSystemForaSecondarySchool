namespace SchoolManagementSystem.Models
{
    public class MeetingRequest
    {
        public int Id { get; set; }

        public string ParentId { get; set; }
        public User? Parent { get; set; }

        public string StudentId { get; set; }
        public User? Student { get; set; }

        public DateTime MeetingDate { get; set; }

        public string Note { get; set; }

        public MeetingRequest()
        {
            ParentId = Guid.NewGuid().ToString();
            StudentId = Guid.NewGuid().ToString();
            MeetingDate = DateTime.Now;
            Note = string.Empty;
        }
    }
}
