using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FitnessTracker.Console.Data;
using FitnessTracker.Console.Services;

namespace FitnessTracker.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var services = new ServiceCollection();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<TrainingProgramService>();
            services.AddScoped<ExerciseService>();
            services.AddScoped<ActivityService>();

            var serviceProvider = services.BuildServiceProvider();

            using (var scope = serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                await dbContext.Database.EnsureCreatedAsync();

                var programService = scope.ServiceProvider.GetRequiredService<TrainingProgramService>();
                var exerciseService = scope.ServiceProvider.GetRequiredService<ExerciseService>();
                var activityService = scope.ServiceProvider.GetRequiredService<ActivityService>();

                await MainMenu(programService, exerciseService, activityService);
            }
        }

        static async Task MainMenu(TrainingProgramService programService,
                                   ExerciseService exerciseService,
                                   ActivityService activityService)
        {
            bool exit = false;
            while (!exit)
            {
                System.Console.Clear();
                System.Console.WriteLine("=== ФИТНЕС ТРЕКЕР - Главное меню ===");
                System.Console.WriteLine("1. Управление тренировочными программами");
                System.Console.WriteLine("2. Управление упражнениями");
                System.Console.WriteLine("3. Управление активностями");
                System.Console.WriteLine("4. Просмотр статистики");
                System.Console.WriteLine("5. Выход");
                System.Console.Write("Выберите пункт: ");

                var choice = System.Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await ManagePrograms(programService);
                        break;
                    case "2":
                        await ManageExercises(exerciseService, programService);
                        break;
                    case "3":
                        await ManageActivities(activityService, exerciseService);
                        break;
                    case "4":
                        await ViewStatistics(activityService);
                        break;
                    case "5":
                        exit = true;
                        break;
                    default:
                        System.Console.WriteLine("Неверный выбор. Нажмите любую клавишу...");
                        System.Console.ReadKey();
                        break;
                }
            }
        }

        static async Task ManagePrograms(TrainingProgramService service)
        {
            bool back = false;
            while (!back)
            {
                System.Console.Clear();
                System.Console.WriteLine("=== УПРАВЛЕНИЕ ПРОГРАММАМИ ===");
                System.Console.WriteLine("1. Показать все программы");
                System.Console.WriteLine("2. Показать активные программы");
                System.Console.WriteLine("3. Добавить программу");
                System.Console.WriteLine("4. Обновить программу");
                System.Console.WriteLine("5. Удалить программу");
                System.Console.WriteLine("6. Назад");
                System.Console.Write("Выберите пункт: ");

                var choice = System.Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            var allPrograms = await service.GetAllProgramsAsync();
                            System.Console.WriteLine("\n=== ВСЕ ПРОГРАММЫ ===");
                            foreach (var p in allPrograms)
                                System.Console.WriteLine(p);
                            System.Console.WriteLine("\nНажмите любую клавишу...");
                            System.Console.ReadKey();
                            break;

                        case "2":
                            var activePrograms = await service.GetActiveProgramsAsync();
                            System.Console.WriteLine("\n=== АКТИВНЫЕ ПРОГРАММЫ ===");
                            foreach (var p in activePrograms)
                                System.Console.WriteLine(p);
                            System.Console.WriteLine("\nНажмите любую клавишу...");
                            System.Console.ReadKey();
                            break;

                        case "3":
                            System.Console.Write("Название программы: ");
                            var name = System.Console.ReadLine() ?? "";
                            System.Console.Write("Тип программы (Силовая/Кардио/Растяжка): ");
                            var type = System.Console.ReadLine() ?? "";
                            System.Console.Write("Активна (да/нет): ");
                            var active = System.Console.ReadLine()?.ToLower() == "да";

                            var newProgram = await service.AddProgramAsync(name, type, active);
                            System.Console.WriteLine($"\nПрограмма добавлена! ID: {newProgram.Id}");
                            System.Console.WriteLine("Нажмите любую клавишу...");
                            System.Console.ReadKey();
                            break;

                        case "4":
                            System.Console.Write("ID программы для обновления: ");
                            if (int.TryParse(System.Console.ReadLine(), out int updateId))
                            {
                                System.Console.Write("Новое название: ");
                                var newName = System.Console.ReadLine() ?? "";
                                System.Console.Write("Новый тип: ");
                                var newType = System.Console.ReadLine() ?? "";
                                System.Console.Write("Активна (да/нет): ");
                                var newActive = System.Console.ReadLine()?.ToLower() == "да";

                                if (await service.UpdateProgramAsync(updateId, newName, newType, newActive))
                                    System.Console.WriteLine("Программа обновлена!");
                                else
                                    System.Console.WriteLine("Программа не найдена!");
                            }
                            System.Console.WriteLine("Нажмите любую клавишу...");
                            System.Console.ReadKey();
                            break;

                        case "5":
                            System.Console.Write("ID программы для удаления: ");
                            if (int.TryParse(System.Console.ReadLine(), out int deleteId))
                            {
                                if (await service.DeleteProgramAsync(deleteId))
                                    System.Console.WriteLine("Программа удалена!");
                                else
                                    System.Console.WriteLine("Программа не найдена!");
                            }
                            System.Console.WriteLine("Нажмите любую клавишу...");
                            System.Console.ReadKey();
                            break;

                        case "6":
                            back = true;
                            break;

                        default:
                            System.Console.WriteLine("Неверный выбор. Нажмите любую клавишу...");
                            System.Console.ReadKey();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine($"Ошибка: {ex.Message}");
                    System.Console.WriteLine("Нажмите любую клавишу...");
                    System.Console.ReadKey();
                }
            }
        }

        static async Task ManageExercises(ExerciseService exerciseService, TrainingProgramService programService)
        {
            bool back = false;
            while (!back)
            {
                System.Console.Clear();
                System.Console.WriteLine("=== УПРАВЛЕНИЕ УПРАЖНЕНИЯМИ ===");
                System.Console.WriteLine("1. Показать все упражнения");
                System.Console.WriteLine("2. Показать активные упражнения");
                System.Console.WriteLine("3. Показать упражнения по программе");
                System.Console.WriteLine("4. Добавить упражнение");
                System.Console.WriteLine("5. Обновить упражнение");
                System.Console.WriteLine("6. Удалить упражнение");
                System.Console.WriteLine("7. Назад");
                System.Console.Write("Выберите пункт: ");

                var choice = System.Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            var allExercises = await exerciseService.GetAllExercisesAsync();
                            System.Console.WriteLine("\n=== ВСЕ УПРАЖНЕНИЯ ===");
                            foreach (var e in allExercises)
                                System.Console.WriteLine(e);
                            System.Console.WriteLine("\nНажмите любую клавишу...");
                            System.Console.ReadKey();
                            break;

                        case "2":
                            var activeExercises = await exerciseService.GetActiveExercisesAsync();
                            System.Console.WriteLine("\n=== АКТИВНЫЕ УПРАЖНЕНИЯ ===");
                            foreach (var e in activeExercises)
                                System.Console.WriteLine(e);
                            System.Console.WriteLine("\nНажмите любую клавишу...");
                            System.Console.ReadKey();
                            break;

                        case "3":
                            System.Console.Write("ID программы: ");
                            if (int.TryParse(System.Console.ReadLine(), out int progId))
                            {
                                var exercises = await exerciseService.GetExercisesByProgramAsync(progId);
                                System.Console.WriteLine($"\n=== УПРАЖНЕНИЯ ДЛЯ ПРОГРАММЫ ID={progId} ===");
                                foreach (var e in exercises)
                                    System.Console.WriteLine(e);
                            }
                            System.Console.WriteLine("\nНажмите любую клавишу...");
                            System.Console.ReadKey();
                            break;

                        case "4":
                            System.Console.Write("Название упражнения: ");
                            var name = System.Console.ReadLine() ?? "";
                            System.Console.Write("ID программы: ");
                            if (int.TryParse(System.Console.ReadLine(), out int programId))
                            {
                                System.Console.Write("Активно (да/нет): ");
                                var active = System.Console.ReadLine()?.ToLower() == "да";

                                var newExercise = await exerciseService.AddExerciseAsync(name, programId, active);
                                System.Console.WriteLine($"\nУпражнение добавлено! ID: {newExercise.Id}");
                            }
                            System.Console.WriteLine("Нажмите любую клавишу...");
                            System.Console.ReadKey();
                            break;

                        case "5":
                            System.Console.Write("ID упражнения для обновления: ");
                            if (int.TryParse(System.Console.ReadLine(), out int updateId))
                            {
                                System.Console.Write("Новое название: ");
                                var newName = System.Console.ReadLine() ?? "";
                                System.Console.Write("Новый ID программы: ");
                                if (int.TryParse(System.Console.ReadLine(), out int newProgramId))
                                {
                                    System.Console.Write("Активно (да/нет): ");
                                    var newActive = System.Console.ReadLine()?.ToLower() == "да";

                                    if (await exerciseService.UpdateExerciseAsync(updateId, newName, newProgramId, newActive))
                                        System.Console.WriteLine("Упражнение обновлено!");
                                    else
                                        System.Console.WriteLine("Упражнение не найдено!");
                                }
                            }
                            System.Console.WriteLine("Нажмите любую клавишу...");
                            System.Console.ReadKey();
                            break;

                        case "6":
                            System.Console.Write("ID упражнения для удаления: ");
                            if (int.TryParse(System.Console.ReadLine(), out int deleteId))
                            {
                                if (await exerciseService.DeleteExerciseAsync(deleteId))
                                    System.Console.WriteLine("Упражнение удалено!");
                                else
                                    System.Console.WriteLine("Упражнение не найдено!");
                            }
                            System.Console.WriteLine("Нажмите любую клавишу...");
                            System.Console.ReadKey();
                            break;

                        case "7":
                            back = true;
                            break;

                        default:
                            System.Console.WriteLine("Неверный выбор. Нажмите любую клавишу...");
                            System.Console.ReadKey();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine($"Ошибка: {ex.Message}");
                    System.Console.WriteLine("Нажмите любую клавишу...");
                    System.Console.ReadKey();
                }
            }
        }

        static async Task ManageActivities(ActivityService activityService, ExerciseService exerciseService)
        {
            bool back = false;
            while (!back)
            {
                System.Console.Clear();
                System.Console.WriteLine("=== УПРАВЛЕНИЕ АКТИВНОСТЯМИ ===");
                System.Console.WriteLine("1. Показать все активности");
                System.Console.WriteLine("2. Добавить активность");
                System.Console.WriteLine("3. Обновить активность");
                System.Console.WriteLine("4. Удалить активность");
                System.Console.WriteLine("5. Назад");
                System.Console.Write("Выберите пункт: ");

                var choice = System.Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            var allActivities = await activityService.GetAllActivitiesAsync();
                            System.Console.WriteLine("\n=== ВСЕ АКТИВНОСТИ ===");
                            foreach (var a in allActivities)
                                System.Console.WriteLine(a);
                            System.Console.WriteLine("\nНажмите любую клавишу...");
                            System.Console.ReadKey();
                            break;

                        case "2":
                            System.Console.Write("ID упражнения: ");
                            if (int.TryParse(System.Console.ReadLine(), out int exerciseId))
                            {
                                System.Console.Write("Дата (гггг-мм-дд): ");
                                if (DateTime.TryParse(System.Console.ReadLine(), out DateTime date))
                                {
                                    System.Console.Write("Количество минут (1-1440): ");
                                    if (int.TryParse(System.Console.ReadLine(), out int minutes))
                                    {
                                        System.Console.Write("Примечания (опционально): ");
                                        var notes = System.Console.ReadLine();

                                        var newActivity = await activityService.AddActivityAsync(exerciseId, date, minutes, notes);
                                        System.Console.WriteLine($"\nАктивность добавлена! ID: {newActivity.Id}");
                                    }
                                }
                            }
                            System.Console.WriteLine("Нажмите любую клавишу...");
                            System.Console.ReadKey();
                            break;

                        case "3":
                            System.Console.Write("ID активности для обновления: ");
                            if (int.TryParse(System.Console.ReadLine(), out int updateId))
                            {
                                System.Console.Write("Новый ID упражнения: ");
                                if (int.TryParse(System.Console.ReadLine(), out int newExerciseId))
                                {
                                    System.Console.Write("Новая дата (гггг-мм-дд): ");
                                    if (DateTime.TryParse(System.Console.ReadLine(), out DateTime newDate))
                                    {
                                        System.Console.Write("Новое количество минут: ");
                                        if (int.TryParse(System.Console.ReadLine(), out int newMinutes))
                                        {
                                            System.Console.Write("Новые примечания: ");
                                            var newNotes = System.Console.ReadLine();

                                            if (await activityService.UpdateActivityAsync(updateId, newExerciseId, newDate, newMinutes, newNotes))
                                                System.Console.WriteLine("Активность обновлена!");
                                            else
                                                System.Console.WriteLine("Активность не найдена!");
                                        }
                                    }
                                }
                            }
                            System.Console.WriteLine("Нажмите любую клавишу...");
                            System.Console.ReadKey();
                            break;

                        case "4":
                            System.Console.Write("ID активности для удаления: ");
                            if (int.TryParse(System.Console.ReadLine(), out int deleteId))
                            {
                                if (await activityService.DeleteActivityAsync(deleteId))
                                    System.Console.WriteLine("Активность удалена!");
                                else
                                    System.Console.WriteLine("Активность не найдена!");
                            }
                            System.Console.WriteLine("Нажмите любую клавишу...");
                            System.Console.ReadKey();
                            break;

                        case "5":
                            back = true;
                            break;

                        default:
                            System.Console.WriteLine("Неверный выбор. Нажмите любую клавишу...");
                            System.Console.ReadKey();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine($"Ошибка: {ex.Message}");
                    System.Console.WriteLine("Нажмите любую клавишу...");
                    System.Console.ReadKey();
                }
            }
        }

        static async Task ViewStatistics(ActivityService activityService)
        {
            bool back = false;
            while (!back)
            {
                System.Console.Clear();
                System.Console.WriteLine("=== СТАТИСТИКА ===");
                System.Console.WriteLine("1. Статистика за день");
                System.Console.WriteLine("2. Статистика за месяц");
                System.Console.WriteLine("3. Назад");
                System.Console.Write("Выберите пункт: ");

                var choice = System.Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            System.Console.Write("Введите дату (гггг-мм-дд): ");
                            if (DateTime.TryParse(System.Console.ReadLine(), out DateTime date))
                            {
                                var (activities, totalMinutes, status, statusColor) =
                                    await activityService.GetActivitiesByDateAsync(date);

                                System.Console.WriteLine($"\n=== СТАТИСТИКА ЗА {date:dd.MM.yyyy} ===");
                                System.Console.WriteLine($"Всего минут: {totalMinutes}");
                                System.Console.WriteLine($"Статус: {status} ({statusColor})");
                                System.Console.WriteLine($"\nДетали:");
                                foreach (var a in activities)
                                    System.Console.WriteLine($"  - {a}");
                            }
                            System.Console.WriteLine("\nНажмите любую клавишу...");
                            System.Console.ReadKey();
                            break;

                        case "2":
                            System.Console.Write("Введите год: ");
                            if (int.TryParse(System.Console.ReadLine(), out int year))
                            {
                                System.Console.Write("Введите месяц (1-12): ");
                                if (int.TryParse(System.Console.ReadLine(), out int month))
                                {
                                    var monthlyStats = await activityService.GetActivitiesByMonthAsync(year, month);

                                    System.Console.WriteLine($"\n=== СТАТИСТИКА ЗА {month}.{year} ===");
                                    if (monthlyStats.Count == 0)
                                    {
                                        System.Console.WriteLine("Нет активностей за этот месяц");
                                    }
                                    else
                                    {
                                        foreach (var kvp in monthlyStats.OrderBy(k => k.Key))
                                        {
                                            System.Console.WriteLine($"{kvp.Key:dd.MM.yyyy}: " +
                                                $"{kvp.Value.TotalMinutes} минут, " +
                                                $"{kvp.Value.Status} ({kvp.Value.StatusColor})");
                                        }
                                    }
                                }
                            }
                            System.Console.WriteLine("\nНажмите любую клавишу...");
                            System.Console.ReadKey();
                            break;

                        case "3":
                            back = true;
                            break;

                        default:
                            System.Console.WriteLine("Неверный выбор. Нажмите любую клавишу...");
                            System.Console.ReadKey();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine($"Ошибка: {ex.Message}");
                    System.Console.WriteLine("Нажмите любую клавишу...");
                    System.Console.ReadKey();
                }
            }
        }
    }
}