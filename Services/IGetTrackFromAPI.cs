using DogRallyMVC.Models;

namespace DogRallyMVC.Services
{
    public interface IGetTrackFromAPI
    {
        Task<List<TrackExerciseDTO>> GetTrack(HttpClient client, int trackID);
    }
}
