using DogRallyMVC.Models;
using Newtonsoft.Json;
using System.Text;

namespace DogRallyMVC.Services
{
    public class PostTrackToAPI : IPostTrackToAPI
    {
        public async Task<HttpResponseMessage> PostTrack(TrackExerciseViewModelDTO tevm, HttpClient client)
        {
            try
            {
                var json = JsonConvert.SerializeObject(tevm);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("https://localhost:7183/Tracks/CreateTrack", content);
                return response;
            }
            catch (Exception ex)
            {
                // Log error or handle it as needed
                throw new InvalidOperationException("Error serializing the track data.", ex);
            }
        }
    }
}
