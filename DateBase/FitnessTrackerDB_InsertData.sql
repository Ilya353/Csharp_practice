USE FitnessTrackerDB;
GO

INSERT INTO TrainingPrograms (Name, Type, IsActive, CreatedAt, UpdatedAt)
VALUES 
    ('Силовая тренировка', 'Силовая', 1, GETDATE(), GETDATE()),
    ('Кардио тренировка', 'Кардио', 1, GETDATE(), GETDATE()),
    ('Йога', 'Растяжка', 1, GETDATE(), GETDATE()),
    ('Функциональный тренинг', 'Силовая', 1, GETDATE(), GETDATE()),
    ('Бег', 'Кардио', 0, GETDATE(), GETDATE()),  
    ('Пилатес', 'Растяжка', 1, GETDATE(), GETDATE()),
    ('Кроссфит', 'Силовая', 0, GETDATE(), GETDATE()), 
    ('Плавание', 'Кардио', 1, GETDATE(), GETDATE());
GO

USE FitnessTrackerDB;
GO

INSERT INTO Exercises (Name, ProgramId, IsActive, CreatedAt, UpdatedAt)
VALUES 
    -- Упражнения для "Силовая тренировка"
    ('Приседания со штангой', 1, 1, GETDATE(), GETDATE()),
    ('Жим лежа', 1, 1, GETDATE(), GETDATE()),
    ('Становая тяга', 1, 1, GETDATE(), GETDATE()),
    ('Подтягивания', 1, 1, GETDATE(), GETDATE()),
    ('Жим гантелей сидя', 1, 1, GETDATE(), GETDATE()),
    
    -- Упражнения для "Кардио тренировка"
    ('Бег на дорожке', 2, 1, GETDATE(), GETDATE()),
    ('Велотренажер', 2, 1, GETDATE(), GETDATE()),
    ('Эллиптический тренажер', 2, 0, GETDATE(), GETDATE()), 
    ('Гребной тренажер', 2, 1, GETDATE(), GETDATE()),
    
    -- Упражнения для "Йога" (ProgramId = 3)
    ('Собака мордой вниз', 3, 1, GETDATE(), GETDATE()),
    ('Воин I', 3, 1, GETDATE(), GETDATE()),
    ('Поза дерева', 3, 1, GETDATE(), GETDATE()),
    ('Поза моста', 3, 0, GETDATE(), GETDATE()),
    ('Поза голубя', 3, 1, GETDATE(), GETDATE()),
    
    -- Упражнения для "Функциональный тренинг"
    ('Берпи', 4, 1, GETDATE(), GETDATE()),
    ('Выпады с гантелями', 4, 1, GETDATE(), GETDATE()),
    ('Планка', 4, 1, GETDATE(), GETDATE()),
    ('Боковые выпады', 4, 1, GETDATE(), GETDATE()),
    ('Запрыгивания на тумбу', 4, 0, GETDATE(), GETDATE()),
    
    -- Упражнения для "Бег"
    ('Бег 5 км', 5, 1, GETDATE(), GETDATE()),
    ('Бег 10 км', 5, 0, GETDATE(), GETDATE()),
    
    -- Упражнения для "Пилатес"
    ('Сотня', 6, 1, GETDATE(), GETDATE()),
    ('Ножницы', 6, 1, GETDATE(), GETDATE()),
    ('Плавающий', 6, 1, GETDATE(), GETDATE()),
    
    -- Упражнения для "Кроссфит"
    ('Рывок штанги', 7, 1, GETDATE(), GETDATE()),
    ('Махи гирей', 7, 0, GETDATE(), GETDATE()),
    
    -- Упражнения для "Плавание"
    ('Брасс', 8, 1, GETDATE(), GETDATE()),
    ('Кроль', 8, 1, GETDATE(), GETDATE()),
    ('Баттерфляй', 8, 0, GETDATE(), GETDATE());
GO

DECLARE @Today DATE = GETDATE();

INSERT INTO Activities (ExerciseId, ActivityDate, Minutes, Notes, CreatedAt, UpdatedAt)
VALUES 
    -- Активности за сегодня
    (1, @Today, 45, 'Хорошая тренировка, вес 50кг', GETDATE(), GETDATE()),
    (6, @Today, 30, 'Самочувствие отличное', GETDATE(), GETDATE()),
    (11, @Today, 15, 'Утренняя йога', GETDATE(), GETDATE()),
    
    -- Активности за вчера
    (9, DATEADD(DAY, -1, @Today), 60, 'Йога утром', GETDATE(), GETDATE()),
    (14, DATEADD(DAY, -1, @Today), 20, 'Интенсивная тренировка', GETDATE(), GETDATE()),
    (7, DATEADD(DAY, -1, @Today), 35, 'Велотренировка', GETDATE(), GETDATE()),
    
    -- Активности за 2 дня назад
    (2, DATEADD(DAY, -2, @Today), 40, 'Грудь + трицепс', GETDATE(), GETDATE()),
    (8, DATEADD(DAY, -2, @Today), 25, 'Кардио после силовой', GETDATE(), GETDATE()),
    (18, DATEADD(DAY, -2, @Today), 30, 'Функциональная тренировка', GETDATE(), GETDATE()),
    
    -- Активности за 3 дня назад
    (3, DATEADD(DAY, -3, @Today), 50, 'Тяжелая тренировка', GETDATE(), GETDATE()),
    (19, DATEADD(DAY, -3, @Today), 25, 'Боковые выпады', GETDATE(), GETDATE()),
    
    -- Активности за 4 дня назад
    (10, DATEADD(DAY, -4, @Today), 15, 'Легкая растяжка', GETDATE(), GETDATE()),
    (22, DATEADD(DAY, -4, @Today), 40, 'Плавательный тренинг', GETDATE(), GETDATE()),
    
    -- Активности за 5 дней назад 
    (15, DATEADD(DAY, -5, @Today), 95, 'Очень интенсивно, вес 10кг гантели', GETDATE(), GETDATE()),
    (6, DATEADD(DAY, -5, @Today), 40, 'Бег вечером', GETDATE(), GETDATE()),
    
    -- Активности за 6 дней назад
    (4, DATEADD(DAY, -6, @Today), 35, '10 подходов по 5 раз', GETDATE(), GETDATE()),
    (20, DATEADD(DAY, -6, @Today), 25, 'Пилатес', GETDATE(), GETDATE()),
    
    -- Активности за 7 дней назад 
    (12, DATEADD(DAY, -7, @Today), 10, 'Быстрая разминка', GETDATE(), GETDATE()),
    (26, DATEADD(DAY, -7, @Today), 45, 'Плавание брассом', GETDATE(), GETDATE()),
    
    -- Активности за 8 дней назад
    (16, DATEADD(DAY, -8, @Today), 120, 'Марафон планки!', GETDATE(), GETDATE()),
    (5, DATEADD(DAY, -8, @Today), 60, 'Жим гантелей', GETDATE(), GETDATE()),
    
    -- Активности за 9 дней назад
    (13, DATEADD(DAY, -9, @Today), 30, 'Поза голубя', GETDATE(), GETDATE()),
    (27, DATEADD(DAY, -9, @Today), 50, 'Кроль', GETDATE(), GETDATE()),
    
    -- Активности за 10 дней назад
    (17, DATEADD(DAY, -10, @Today), 45, 'Выпады с гантелями', GETDATE(), GETDATE()),
    (21, DATEADD(DAY, -10, @Today), 20, 'Запрыгивания', GETDATE(), GETDATE()),
    
    -- Активности за 15 дней назад
    (1, DATEADD(DAY, -15, @Today), 55, 'Приседания', GETDATE(), GETDATE()),
    (7, DATEADD(DAY, -15, @Today), 30, 'Велотренажер', GETDATE(), GETDATE());
GO
