using DogRallyMVC.Models;

namespace DogRallyMVC.Services
{
    public interface IGetExercisesFromAPI
    {
        Task<List<ExerciseDTO>> GetExercises(HttpClient client);
    }
}