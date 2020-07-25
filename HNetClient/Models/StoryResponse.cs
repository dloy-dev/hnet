using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularClient.Models
{
    public class StoryResponse
    {
        public IEnumerable<Story> Stories { get; set; }
        public StoryParameters Parameters { get; set; }
    }
}
