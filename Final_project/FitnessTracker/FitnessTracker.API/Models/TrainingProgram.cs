using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.Console.Models
{
    public class TrainingProgram
    {
        // Первичный ключ.
        public int Id { get; set; }

        // Название программы.
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        // Тип программы.
        [Required]
        [StringLength(50)]
        public string Type { get; set; } = string.Empty;

        // Статус активности.
        public bool IsActive { get; set; } = true;

        // Дата создания записи.
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Дата последнего обновления.
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Коллекция связанных упражнений.
        public ICollection<Exercise>? Exercises { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}, Название: {Name}, Тип: {Type}, Активна: {(IsActive ? "Да" : "Нет")}";
        }
    }
}
