using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.API.DTOs
{
    // Для отправки данных клиенту.
    public class TrainingProgramDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int ExercisesCount { get; set; } 
    }

    // Для получения данных от клиента.
    public class CreateTrainingProgramDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Type { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
    }

    // Для обновления данных (PUT).
    public class UpdateTrainingProgramDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Type { get; set; } = string.Empty;

        public bool IsActive { get; set; }
    }
}
