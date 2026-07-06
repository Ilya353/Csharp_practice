using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace FitnessTracker.Console.Models
{
    public class Exercise
    {
        // Первичный ключ.
        public int Id { get; set; }

        // Название упражнения.
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        // Внешний ключ.
        [Required]
        public int ProgramId { get; set; }

        // Статус активности.
        public bool IsActive { get; set; } = true;

        // Дата создания записи.
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Дата последнего обновления.
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Ссылка на связанную программу.
        public TrainingProgram? Program { get; set; }

        // Коллекция связанных активностей. 
        public ICollection<Activity>? Activities { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}, Название: {Name}, Программа: {Program?.Name ?? "N/A"}, Активно: {(IsActive ? "Да" : "Нет")}";
        }
    }
}
