using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.API.DTOs
{
    public class ActivityDto
    {
        public int Id { get; set; }
        public int ExerciseId { get; set; }
        public string? ExerciseName { get; set; } 
        public DateTime ActivityDate { get; set; }
        public int Minutes { get; set; }
        public string? Notes { get; set; }
    }

    public class CreateActivityDto
    {
        [Required]
        public int ExerciseId { get; set; }

        [Required]
        public DateTime ActivityDate { get; set; }

        [Required]
        [Range(1, 1440)]
        public int Minutes { get; set; }

        [StringLength(200)]
        public string? Notes { get; set; }
    }

    public class UpdateActivityDto
    {
        [Required]
        public int ExerciseId { get; set; }

        [Required]
        public DateTime ActivityDate { get; set; }

        [Required]
        [Range(1, 1440)]
        public int Minutes { get; set; }

        [StringLength(200)]
        public string? Notes { get; set; }
    }

    // Специальный DTO для статистики.
    public class ActivitySummaryDto
    {
        public DateTime Date { get; set; }
        public int TotalMinutes { get; set; }
        public string Status { get; set; } = string.Empty;
        public string StatusColor { get; set; } = string.Empty;
        public List<ActivityDto> Activities { get; set; } = new();
    }
}
