using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.API.DTOs
{
    public class ExerciseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int ProgramId { get; set; }
        public string? ProgramName { get; set; } 
        public bool IsActive { get; set; }
    }

    public class CreateExerciseDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public int ProgramId { get; set; }

        public bool IsActive { get; set; } = true;
    }

    public class UpdateExerciseDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public int ProgramId { get; set; }

        public bool IsActive { get; set; }
    }
}
