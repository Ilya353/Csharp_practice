using Microsoft.EntityFrameworkCore;
using FitnessTracker.Console.Models;

namespace FitnessTracker.Console.Data
{
    /// <summary>
    /// Контекст базы данных для приложения Fitness Tracker.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            // Отключаем отслеживание для запросов только для чтения.
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public DbSet<TrainingProgram> TrainingPrograms { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<Activity> Activities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Конфигурация для TrainingProgram.
            modelBuilder.Entity<TrainingProgram>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValue(true);

                entity.Property(e => e.CreatedAt)
                    .IsRequired()
                    .HasDefaultValue(DateTime.Now);

                entity.Property(e => e.UpdatedAt)
                    .IsRequired()
                    .HasDefaultValue(DateTime.Now);

                // Индексы.
                entity.HasIndex(e => e.Name)
                    .HasDatabaseName("IX_TrainingPrograms_Name");

                entity.HasIndex(e => e.IsActive)
                    .HasDatabaseName("IX_TrainingPrograms_IsActive");

                // Отношения.
                entity.HasMany(e => e.Exercises)
                    .WithOne(e => e.Program)
                    .HasForeignKey(e => e.ProgramId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Конфигурация для Exercise.
            modelBuilder.Entity<Exercise>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ProgramId)
                    .IsRequired();

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValue(true);

                entity.Property(e => e.CreatedAt)
                    .IsRequired()
                    .HasDefaultValue(DateTime.Now);

                entity.Property(e => e.UpdatedAt)
                    .IsRequired()
                    .HasDefaultValue(DateTime.Now);

                // Индексы.
                entity.HasIndex(e => e.ProgramId)
                    .HasDatabaseName("IX_Exercises_ProgramId");

                entity.HasIndex(e => e.IsActive)
                    .HasDatabaseName("IX_Exercises_IsActive");

                entity.HasIndex(e => new { e.ProgramId, e.IsActive })
                    .HasDatabaseName("IX_Exercises_ProgramId_IsActive");

                // Отношения.
                entity.HasOne(e => e.Program)
                    .WithMany(p => p.Exercises)
                    .HasForeignKey(e => e.ProgramId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.Activities)
                    .WithOne(a => a.Exercise)
                    .HasForeignKey(a => a.ExerciseId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Конфигурация для Activity.
            modelBuilder.Entity<Activity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ExerciseId)
                    .IsRequired();

                entity.Property(e => e.ActivityDate)
                    .IsRequired()
                    .HasColumnType("date");

                entity.Property(e => e.Minutes)
                    .IsRequired();

                entity.Property(e => e.Notes)
                    .HasMaxLength(200);

                entity.Property(e => e.CreatedAt)
                    .IsRequired()
                    .HasDefaultValue(DateTime.Now);

                entity.Property(e => e.UpdatedAt)
                    .IsRequired()
                    .HasDefaultValue(DateTime.Now);

                // Индексы.
                entity.HasIndex(e => e.ExerciseId)
                    .HasDatabaseName("IX_Activities_ExerciseId");

                entity.HasIndex(e => e.ActivityDate)
                    .HasDatabaseName("IX_Activities_ActivityDate");

                entity.HasIndex(e => new { e.ActivityDate, e.ExerciseId })
                    .HasDatabaseName("IX_Activities_Date_Exercise");

                // // Отношения.
                entity.HasOne(e => e.Exercise)
                    .WithMany(a => a.Activities)
                    .HasForeignKey(e => e.ExerciseId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Проверочные ограничения.
                entity.ToTable(tb => tb.HasCheckConstraint("CHK_Minutes_Positive", "Minutes > 0"));
                entity.ToTable(tb => tb.HasCheckConstraint("CHK_Minutes_Max", "Minutes <= 1440"));
            });
        }
    }
}