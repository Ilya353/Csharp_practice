using Microsoft.EntityFrameworkCore;
using FitnessTracker.Console.Data;
using FitnessTracker.Console.Services;

var builder = WebApplication.CreateBuilder(args);

// Êîíòðîëëåðû.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Íàñòðîéêà Swagger ñ äîêóìåíòàöèåé.
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Fitness Tracker API",
        Version = "v1",
        Description = "API äëÿ ó÷åòà ëè÷íûõ òðåíèðîâîê è ôèçè÷åñêîé àêòèâíîñòè",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Support",
            Email = "support@fitness-tracker.com"
        }
    });
});

// Íàñòðîéêà CORS (ðàçðåøàåì çàïðîñû ñ ëþáîãî èñòî÷íèêà).
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

// Ïîäêëþ÷åíèå ê áàçå äàííûõ.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Ðåãèñòðàöèÿ ñåðâèñîâ (âíåäðåíèå çàâèñèìîñòåé).
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
