namespace SchoolManagementSystem.Models
{
    public class Result
    {
        public int Id { get; set; }
        public User Student { get; set; }
        public List<Score> Scores { get; set; }
        public string Remark { get; set; }

        public Result()
        {
            Student = new User();
            Scores = new List<Score>();
            Remark = string.Empty;
        }
    }
}
