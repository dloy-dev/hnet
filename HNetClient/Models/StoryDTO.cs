namespace AngularClient.Models
{
    public class StoryDTO
    {
        public string by { get; set; }
        public int decendents { get; set; }
        public int id { get; set; }
        public int[] kids { get; set; }
        public int score { get; set; }
        public double time { get; set; }
        public string title { get; set; }
        public string type { get; set; }
        public string url { get; set; }
    }
}
