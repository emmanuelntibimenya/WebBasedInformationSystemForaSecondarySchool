namespace SchoolManagementSystem.Models
{
    public class LearnerProfile
    {
        public  int ID { get; set; }
        public User Student { get; set; }
        public string Note { get; set; }
        public LearnerProfile()
        {
            Student = new User();
            Note = string.Empty;
            
        }
    }
}
