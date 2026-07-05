using Microsoft.EntityFrameworkCore;
using FitnessTracker.Console.Data;
using FitnessTracker.Console.Services;

var builder = WebApplication.CreateBuilder(args);

// Контроллеры.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Настройка Swagger с документацией.
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Fitness Tracker API",
        Version = "v1",
        Description = "API для учета личных тренировок и физической активности",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Support",
            Email = "support@fitness-tracker.com"
        }
    });
});

// Настройка CORS (разрешаем запросы с любого источника).
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// Подключение к базе данных.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Регистрация сервисов (внедрение зависимостей).
builder.Services.AddScoped<TrainingProgramService>();
builder.Services.AddScoped<ExerciseService>();
builder.Services.AddScoped<ActivityService>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
