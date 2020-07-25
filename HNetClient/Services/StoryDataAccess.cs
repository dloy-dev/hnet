using System.Net.Http;
using System.Threading.Tasks;

namespace AngularClient.Services
{
    public interface IStoryDataAccess 
    {
        Task<string> GetExternalResponseAsync(string url);
    }

    public class StoryDataAccess : IStoryDataAccess
    {
        public async Task<string> GetExternalResponseAsync(string url)
        {
            var client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }
    }
}
