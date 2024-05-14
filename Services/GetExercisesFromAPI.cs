using DogRallyMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;

namespace DogRallyMVC.Services
{
    public class GetExercisesFromAPI : IGetExercisesFromAPI
    {
        public async Task<List<ExerciseDTO>> GetExercises(HttpClient client)
        {       
            var url = $"https://localhost:7183/Tracks/ReadExercises";

            try
            {
                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var exercises = JsonConvert.DeserializeObject<List<ExerciseDTO>>(responseBody);
                    return exercises;
                }
                else
                {
                    Console.WriteLine($"Anmodningen mislykkedes med statuskode: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Der opstod en undtagelse: {ex.Message}");
            }
            return null;
        }
    }
}

