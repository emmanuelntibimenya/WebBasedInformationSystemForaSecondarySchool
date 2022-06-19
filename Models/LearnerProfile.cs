namespace SchoolManagementSystem.Models
{
    public class LearnerProfile
    {
        public  int ID { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public LearnerProfile()
        {
            Note = string.Empty;

        }
    }
}
