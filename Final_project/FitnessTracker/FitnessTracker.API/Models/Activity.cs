using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.Console.Models
{
    public class Activity
    {
        // Первичный ключ.
        public int Id { get; set; }

        // Внешний ключ.
        [Required]
        public int ExerciseId { get; set; }

        // Дата выполнения тренировки.
        [Required]
        public DateTime ActivityDate { get; set; }

        // Длительность тренировки в минутах.
        [Required]
        [Range(1, 1440)]
        public int Minutes { get; set; }

        // Примечание.
        [StringLength(200)]
        public string? Notes { get; set; }

        // Дата создания записи.
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Дата последнего обновления.
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Ссылка на связанное упражнение.
        public Exercise? Exercise { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}, Упражнение: {Exercise?.Name ?? "N/A"}, " +
                   $"Дата: {ActivityDate:dd.MM.yyyy}, Минут: {Minutes}, " +
                   $"Примечания: {Notes ?? "Нет"}";
        }
    }
}
