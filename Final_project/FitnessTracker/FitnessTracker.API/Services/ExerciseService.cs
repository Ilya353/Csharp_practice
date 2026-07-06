using Microsoft.EntityFrameworkCore;
using FitnessTracker.Console.Data;
using FitnessTracker.Console.Models;

namespace FitnessTracker.Console.Services
{
    /// <summary>
    /// Бизнес-логика для работы с таблицей Exercises.
    /// <summary>
    public class ExerciseService
    {
        // Контекст базы данных.
        private readonly ApplicationDbContext _context;
        public ExerciseService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Добавление нового упражнения.
        public async Task<Exercise> AddExerciseAsync(string name, int programId, bool isActive = true)
        {
            // Проверка существования программы.
            var program = await _context.TrainingPrograms.FindAsync(programId);
            if (program == null)
                throw new Exception($"Программа с ID {programId} не найдена");

            // Создание нового упражнения.
            var exercise = new Exercise
            {
                Name = name,
                ProgramId = programId,
                IsActive = isActive,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            // Сохранение в БД.
            _context.Exercises.Add(exercise);
            await _context.SaveChangesAsync();
            return exercise;
        }

        // Получение всех упражнений.
        public async Task<List<Exercise>> GetAllExercisesAsync()
        {
            return await _context.Exercises
                .Include(e => e.Program)
                .OrderBy(e => e.Name)
                .ToListAsync();
        }

        // Получение только активных упражнений.
        public async Task<List<Exercise>> GetActiveExercisesAsync()
        {
            return await _context.Exercises
                .Include(e => e.Program)
                .Where(e => e.IsActive && e.Program.IsActive)
                .ToListAsync();
        }

        //Получение упражнений по ID программы.
        public async Task<List<Exercise>> GetExercisesByProgramAsync(int programId)
        {
            return await _context.Exercises
                .Where(e => e.ProgramId == programId)
                .Include(e => e.Program)
                .ToListAsync();
        }

        // Обновление существующего упражнения.
        public async Task<bool> UpdateExerciseAsync(int id, string name, int programId, bool isActive)
        {
            // Поиск упражнения.
            var exercise = await _context.Exercises.FindAsync(id);
            if (exercise == null) return false;

            // Проверка существования новой программы.
            var program = await _context.TrainingPrograms.FindAsync(programId);
            if (program == null)
                throw new Exception($"Программа с ID {programId} не найдена");

            // Обновление поля.
            exercise.Name = name;
            exercise.ProgramId = programId;
            exercise.IsActive = isActive;
            exercise.UpdatedAt = DateTime.Now;

            // Сохранение в БД.
            await _context.SaveChangesAsync();
            return true;
        }

        // Удаление упражнения.
        public async Task<bool> DeleteExerciseAsync(int id)
        {
            // Поиск упражнения.
            var exercise = await _context.Exercises.FindAsync(id);
            if (exercise == null) return false;

            // Удаление из БД.
            _context.Exercises.Remove(exercise);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}