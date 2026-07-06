using Microsoft.EntityFrameworkCore;
using FitnessTracker.Console.Data;
using FitnessTracker.Console.Models;

namespace FitnessTracker.Console.Services
{
    /// <summary>
    /// Бизнес-логика для работы с таблицей Activities.
    /// <summary>
    public class ActivityService
    {
        // Контекст базы данных.
        private readonly ApplicationDbContext _context;
        public ActivityService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Добавление новой активности.
        public async Task<Activity> AddActivityAsync(int exerciseId, DateTime date, int minutes, string? notes = null)
        {
            // Проверка существования упражнения.
            var exercise = await _context.Exercises
                .Include(e => e.Program)
                .FirstOrDefaultAsync(e => e.Id == exerciseId);

            if (exercise == null)
                throw new Exception($"Упражнение с ID {exerciseId} не найдено");

            // Проверка активности упражнения и программы.
            if (!exercise.IsActive || !exercise.Program.IsActive)
                throw new Exception("Неактивное упражнение нельзя выбрать");

            // Проверка даты.
            if (date.Date > DateTime.Now.Date)
                throw new Exception("Дата активности не может быть в будущем");

            // Проверка суммарного времяни за день.
            var dateOnly = date.Date;
            var dailyTotal = await _context.Activities
                .Where(a => a.ActivityDate.Date == dateOnly)
                .SumAsync(a => a.Minutes);

            if (dailyTotal + minutes > 1440)
                throw new Exception("Суммарная длительность активностей за день не может превышать 1440 минут");

            // Создание новой активности.
            var activity = new Activity
            {
                ExerciseId = exerciseId,
                ActivityDate = date,
                Minutes = minutes,
                Notes = notes,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            // Сохранение в БД.
            _context.Activities.Add(activity);
            await _context.SaveChangesAsync();
            return activity;
        }

        // Получение всех активностей.
        public async Task<List<Activity>> GetAllActivitiesAsync()
        {
            return await _context.Activities
                .Include(a => a.Exercise)
                .ThenInclude(e => e.Program)
                .OrderByDescending(a => a.ActivityDate)
                .ToListAsync();
        }

        // Получение активностей за конкретный день, общее время, статус.
        public async Task<(List<Activity> Activities, int TotalMinutes, string Status, string StatusColor)>
            GetActivitiesByDateAsync(DateTime date)
        {
            var dateOnly = date.Date;

            // Получение всех активностей за указанную дату.
            var activities = await _context.Activities
                .Include(a => a.Exercise)
                .ThenInclude(e => e.Program)
                .Where(a => a.ActivityDate.Date == dateOnly)
                .OrderBy(a => a.ActivityDate)
                .ToListAsync();

            // Общее время активностей.
            var totalMinutes = activities.Sum(a => a.Minutes);

            // Статус активности.
            string status, statusColor;
            if (totalMinutes < 30)
            {
                status = "Низкая активность";
                statusColor = "Желтый";
            }
            else if (totalMinutes <= 90)
            {
                status = "Активность в норме";
                statusColor = "Зеленый";
            }
            else
            {
                status = "Высокая активность";
                statusColor = "Красный";
            }

            return (activities, totalMinutes, status, statusColor);
        }

        // Получение статистики активности за месяц.
        public async Task<Dictionary<DateTime, (int TotalMinutes, string Status, string StatusColor)>>
            GetActivitiesByMonthAsync(int year, int month)
        {
            // Определение границы месяца.
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            //Все активности за месяц.
            var activities = await _context.Activities
                .Where(a => a.ActivityDate >= startDate && a.ActivityDate <= endDate)
                .ToListAsync();

            var result = new Dictionary<DateTime, (int, string, string)>();

            // Группировка активностей по дням.
            var dailyGroups = activities
                .GroupBy(a => a.ActivityDate.Date)
                .Select(g => new { Date = g.Key, TotalMinutes = g.Sum(a => a.Minutes) });

            // Определение статуса для каждого дня.
            foreach (var group in dailyGroups)
            {
                string status, statusColor;
                if (group.TotalMinutes < 30)
                {
                    status = "Низкая активность";
                    statusColor = "Желтый";
                }
                else if (group.TotalMinutes <= 90)
                {
                    status = "Активность в норме";
                    statusColor = "Зеленый";
                }
                else
                {
                    status = "Высокая активность";
                    statusColor = "Красный";
                }

                result[group.Date] = (group.TotalMinutes, status, statusColor);
            }

            return result;
        }

        // Обновление существующей активности.
        public async Task<bool> UpdateActivityAsync(int id, int exerciseId, DateTime date, int minutes, string? notes = null)
        {
            // Поиск активности.
            var activity = await _context.Activities
                .Include(a => a.Exercise)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (activity == null) return false;

            // Если текущее упражнение неактивно, запрещаем редактирование.
            if (!activity.Exercise.IsActive)
                throw new Exception("Нельзя редактировать активность с неактивным упражнением");

            // Проверка нового упражнения.
            var exercise = await _context.Exercises
                .Include(e => e.Program)
                .FirstOrDefaultAsync(e => e.Id == exerciseId);

            if (exercise == null)
                throw new Exception($"Упражнение с ID {exerciseId} не найдено");

            // Проверяем активность упражнения.
            if (activity.ExerciseId != exerciseId && !exercise.IsActive)
                throw new Exception("Нельзя изменить упражнение на неактивное");

            // Проверка суммарного времяни за день.
            var dateOnly = date.Date;
            var dailyTotal = await _context.Activities
                .Where(a => a.ActivityDate.Date == dateOnly && a.Id != id)
                .SumAsync(a => a.Minutes);

            if (dailyTotal + minutes > 1440)
                throw new Exception("Суммарная длительность активностей за день не может превышать 1440 минут");

            // Обновление поля.
            activity.ExerciseId = exerciseId;
            activity.ActivityDate = date;
            activity.Minutes = minutes;
            activity.Notes = notes;
            activity.UpdatedAt = DateTime.Now;

            // Сохранение в БД.
            await _context.SaveChangesAsync();
            return true;
        }

        // Удаление активности.
        public async Task<bool> DeleteActivityAsync(int id)
        {
            // Поиск активности.
            var activity = await _context.Activities.FindAsync(id);
            if (activity == null) return false;

            // Удаление из БД.
            _context.Activities.Remove(activity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
