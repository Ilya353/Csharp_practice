using Microsoft.EntityFrameworkCore;
using FitnessTracker.Console.Data;
using FitnessTracker.Console.Models;

namespace FitnessTracker.Console.Services
{
    /// <summary>
    /// Бизнес-логика для работы с таблицей TrainingPrograms.
    /// <summary>
    public class TrainingProgramService
    {
        // Контекст базы данных.
        private readonly ApplicationDbContext _context;
        public TrainingProgramService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Добавление новой тренировочной программы.
        public async Task<TrainingProgram> AddProgramAsync(string name, string type, bool isActive = true)
        {
            // Создание новой программы.
            var program = new TrainingProgram
            {
                Name = name,
                Type = type,
                IsActive = isActive,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            // Сохранение в БД.
            _context.TrainingPrograms.Add(program);
            await _context.SaveChangesAsync();
            return program;
        }

        // Получение всех тренировочных программ.
        public async Task<List<TrainingProgram>> GetAllProgramsAsync()
        {
            return await _context.TrainingPrograms
                .Include(p => p.Exercises)
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        // Получение активных тренировочных программ.
        public async Task<List<TrainingProgram>> GetActiveProgramsAsync()
        {
            return await _context.TrainingPrograms
                .Where(p => p.IsActive)
                .Include(p => p.Exercises)
                .ToListAsync();
        }

        // Обновление существующей тренировочной программы.
        public async Task<bool> UpdateProgramAsync(int id, string name, string type, bool isActive)
        {
            // Поиск программы
            var program = await _context.TrainingPrograms.FindAsync(id);
            if (program == null) return false;

            // Обновление поля.
            program.Name = name;
            program.Type = type;
            program.IsActive = isActive;
            program.UpdatedAt = DateTime.Now;

            // Сохранение в БД.
            await _context.SaveChangesAsync();
            return true;
        }

        // Удаление тренировочной программы.
        public async Task<bool> DeleteProgramAsync(int id)
        {
            // Поиск программы.
            var program = await _context.TrainingPrograms.FindAsync(id);
            if (program == null) return false;

            // Удаление из БД.
            _context.TrainingPrograms.Remove(program);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
