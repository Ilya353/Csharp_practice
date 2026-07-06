using Microsoft.AspNetCore.Mvc;
using FitnessTracker.Console.Services;
using FitnessTracker.API.DTOs;

namespace FitnessTracker.API.Controllers
{
    /// <summary>
    /// Контроллер для управления упражнениями..
    /// <summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ExercisesController : ControllerBase
    {
        private readonly ExerciseService _service;

        public ExercisesController(ExerciseService service)
        {
            _service = service;
        }

        // Получить все упражнения.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExerciseDto>>> GetAll()
        {
            var exercises = await _service.GetAllExercisesAsync();
            var result = exercises.Select(e => new ExerciseDto
            {
                Id = e.Id,
                Name = e.Name,
                ProgramId = e.ProgramId,
                ProgramName = e.Program?.Name,
                IsActive = e.IsActive
            });
            return Ok(result);
        }

        // Получить упражнение по ID.
        [HttpGet("{id}")]
        public async Task<ActionResult<ExerciseDto>> GetById(int id)
        {
            var exercises = await _service.GetAllExercisesAsync();
            var exercise = exercises.FirstOrDefault(e => e.Id == id);
            if (exercise == null)
                return NotFound();

            return Ok(new ExerciseDto
            {
                Id = exercise.Id,
                Name = exercise.Name,
                ProgramId = exercise.ProgramId,
                ProgramName = exercise.Program?.Name,
                IsActive = exercise.IsActive
            });
        }

        // Создать новое упражнение.
        [HttpPost]
        public async Task<ActionResult<ExerciseDto>> Create([FromBody] CreateExerciseDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var exercise = await _service.AddExerciseAsync(dto.Name, dto.ProgramId, dto.IsActive);
                var result = new ExerciseDto
                {
                    Id = exercise.Id,
                    Name = exercise.Name,
                    ProgramId = exercise.ProgramId,
                    ProgramName = exercise.Program?.Name,
                    IsActive = exercise.IsActive
                };
                return CreatedAtAction(nameof(GetById), new { id = exercise.Id }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Обновить упражнение.
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateExerciseDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _service.UpdateExerciseAsync(id, dto.Name, dto.ProgramId, dto.IsActive);
                if (!result)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Удалить упражнение.
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteExerciseAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        // Получить активные упражнения.
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<ExerciseDto>>> GetActive()
        {
            var exercises = await _service.GetActiveExercisesAsync();
            var result = exercises.Select(e => new ExerciseDto
            {
                Id = e.Id,
                Name = e.Name,
                ProgramId = e.ProgramId,
                ProgramName = e.Program?.Name,
                IsActive = e.IsActive
            });
            return Ok(result);
        }

        // Получить упражнения по программе.
        [HttpGet("by-program/{programId}")]
        public async Task<ActionResult<IEnumerable<ExerciseDto>>> GetByProgram(int programId)
        {
            var exercises = await _service.GetExercisesByProgramAsync(programId);
            var result = exercises.Select(e => new ExerciseDto
            {
                Id = e.Id,
                Name = e.Name,
                ProgramId = e.ProgramId,
                ProgramName = e.Program?.Name,
                IsActive = e.IsActive
            });
            return Ok(result);
        }
    }
}