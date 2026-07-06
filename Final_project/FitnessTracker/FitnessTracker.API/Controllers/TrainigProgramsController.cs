using Microsoft.AspNetCore.Mvc;
using FitnessTracker.Console.Services;
using FitnessTracker.API.DTOs;

namespace FitnessTracker.API.Controllers
{
    /// <summary>
    /// Контроллер для управления тренировочными программами.
    /// <summary>
    [ApiController]
    [Route("api/[controller]")]
    public class TrainingProgramsController : ControllerBase
    {
        private readonly TrainingProgramService _service;

        public TrainingProgramsController(TrainingProgramService service)
        {
            _service = service;
        }

        // Получить все тренировочные программы.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrainingProgramDto>>> GetAll()
        {
            var programs = await _service.GetAllProgramsAsync();
            var result = programs.Select(p => new TrainingProgramDto
            {
                Id = p.Id,
                Name = p.Name,
                Type = p.Type,
                IsActive = p.IsActive,
                ExercisesCount = p.Exercises?.Count ?? 0
            });
            return Ok(result);
        }

        // Получить программу по ID.
        [HttpGet("{id}")]
        public async Task<ActionResult<TrainingProgramDto>> GetById(int id)
        {
            var programs = await _service.GetAllProgramsAsync();
            var program = programs.FirstOrDefault(p => p.Id == id);
            if (program == null)
                return NotFound();

            return Ok(new TrainingProgramDto
            {
                Id = program.Id,
                Name = program.Name,
                Type = program.Type,
                IsActive = program.IsActive,
                ExercisesCount = program.Exercises?.Count ?? 0
            });
        }

        // Создать новую программу.
        [HttpPost]
        public async Task<ActionResult<TrainingProgramDto>> Create([FromBody] CreateTrainingProgramDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var program = await _service.AddProgramAsync(dto.Name, dto.Type, dto.IsActive);
            var result = new TrainingProgramDto
            {
                Id = program.Id,
                Name = program.Name,
                Type = program.Type,
                IsActive = program.IsActive,
                ExercisesCount = 0
            };
            return CreatedAtAction(nameof(GetById), new { id = program.Id }, result);
        }

        // Обновить программу.
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTrainingProgramDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.UpdateProgramAsync(id, dto.Name, dto.Type, dto.IsActive);
            if (!result)
                return NotFound();

            return NoContent();
        }

        // Удалить программу.
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteProgramAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        // Получить активные программы.
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<TrainingProgramDto>>> GetActive()
        {
            var programs = await _service.GetActiveProgramsAsync();
            var result = programs.Select(p => new TrainingProgramDto
            {
                Id = p.Id,
                Name = p.Name,
                Type = p.Type,
                IsActive = p.IsActive,
                ExercisesCount = p.Exercises?.Count ?? 0
            });
            return Ok(result);
        }
    }
}
