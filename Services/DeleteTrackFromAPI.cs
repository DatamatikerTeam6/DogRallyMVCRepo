using DogRallyMVC.Models;
using Newtonsoft.Json;

namespace DogRallyMVC.Services
{
    public class DeleteTrackFromAPI : IDeleteTrackFromAPI
    {
        public async Task DeleteTrack(HttpClient client, int id)
        {
            var url = $"https://localhost:7183/Tracks/DeleteTrack/{id}"; // Ensure the URL matches your API routing

            try
            {
                HttpResponseMessage response = await client.DeleteAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    throw new InvalidOperationException($"Failed to delete track: {response.StatusCode} - {errorResponse}");
                }
            }
            catch (Exception ex)
            {
                // Consider logging the exception in a way that's appropriate for your application environment
                // This might involve using a logging framework like NLog, Serilog, or another
                throw new Exception("An error occurred while deleting the track.", ex);
            }
        }
    }
}
