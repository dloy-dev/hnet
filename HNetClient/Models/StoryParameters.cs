namespace AngularClient.Models
{
    public class StoryParameters
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalElements { get; set; }
        public string Search { get; set; }
    }
}
