using DogRallyMVC.Models;

namespace DogRallyMVC.Services
{
    public interface IPostTrackToAPI
    {
        Task<HttpResponseMessage> PostTrack(TrackExerciseViewModelDTO tevm, HttpClient client);
    }
}