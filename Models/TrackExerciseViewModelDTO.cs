using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DogRallyMVC.Models
{
    public class TrackExerciseViewModelDTO
    {
        [JsonPropertyName("exercises")]
        [Required]
        public List<ExerciseDTO> Exercises { get; set; }

        [JsonPropertyName("track")]
        [Required]
        public TrackDTO Track { get; set; }
    }
}
