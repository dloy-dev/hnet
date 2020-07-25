using System;

namespace AngularClient.Models
{
    public class Story
    {
        public string Author { get; set; }
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
    }
}
