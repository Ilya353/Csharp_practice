using Microsoft.AspNetCore.Mvc;
using FitnessTracker.Console.Services;
using FitnessTracker.API.DTOs;

namespace FitnessTracker.API.Controllers
{
    /// <summary>
    /// Контроллер для управления активностями.
    /// <summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ActivitiesController : ControllerBase
    {
        private readonly ActivityService _service;

        public ActivitiesController(ActivityService service)
        {
            _service = service;
        }

        // Получить все активности.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ActivityDto>>> GetAll()
        {
            var activities = await _service.GetAllActivitiesAsync();
            var result = activities.Select(a => new ActivityDto
            {
                Id = a.Id,
                ExerciseId = a.ExerciseId,
                ExerciseName = a.Exercise?.Name,
                ActivityDate = a.ActivityDate,
                Minutes = a.Minutes,
                Notes = a.Notes
            });
            return Ok(result);
        }

        // Получить активность по ID.
        [HttpGet("{id}")]
        public async Task<ActionResult<ActivityDto>> GetById(int id)
        {
            var activities = await _service.GetAllActivitiesAsync();
            var activity = activities.FirstOrDefault(a => a.Id == id);
            if (activity == null)
                return NotFound();

            return Ok(new ActivityDto
            {
                Id = activity.Id,
                ExerciseId = activity.ExerciseId,
                ExerciseName = activity.Exercise?.Name,
                ActivityDate = activity.ActivityDate,
                Minutes = activity.Minutes,
                Notes = activity.Notes
            });
        }

        // Создать новую активность.
        [HttpPost]
        public async Task<ActionResult<ActivityDto>> Create([FromBody] CreateActivityDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var activity = await _service.AddActivityAsync(
                    dto.ExerciseId,
                    dto.ActivityDate,
                    dto.Minutes,
                    dto.Notes);

                var result = new ActivityDto
                {
                    Id = activity.Id,
                    ExerciseId = activity.ExerciseId,
                    ExerciseName = activity.Exercise?.Name,
                    ActivityDate = activity.ActivityDate,
                    Minutes = activity.Minutes,
                    Notes = activity.Notes
                };
                return CreatedAtAction(nameof(GetById), new { id = activity.Id }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Обновить активность.
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateActivityDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _service.UpdateActivityAsync(
                    id,
                    dto.ExerciseId,
                    dto.ActivityDate,
                    dto.Minutes,
                    dto.Notes);

                if (!result)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Удалить активность.
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteActivityAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        // Получить активности за конкретный день с цветовым индикатором.
        [HttpGet("by-date")]
        public async Task<ActionResult<ActivitySummaryDto>> GetByDate([FromQuery] DateTime date)
        {
            var (activities, totalMinutes, status, statusColor) =
                await _service.GetActivitiesByDateAsync(date);

            var result = new ActivitySummaryDto
            {
                Date = date.Date,
                TotalMinutes = totalMinutes,
                Status = status,
                StatusColor = statusColor,
                Activities = activities.Select(a => new ActivityDto
                {
                    Id = a.Id,
                    ExerciseId = a.ExerciseId,
                    ExerciseName = a.Exercise?.Name,
                    ActivityDate = a.ActivityDate,
                    Minutes = a.Minutes,
                    Notes = a.Notes
                }).ToList()
            };
            return Ok(result);
        }

        // Получить статистику активностей за месяц.
        [HttpGet("by-month")]
        public async Task<ActionResult<Dictionary<DateTime, object>>> GetByMonth(
            [FromQuery] int year,
            [FromQuery] int month)
        {
            var monthlyStats = await _service.GetActivitiesByMonthAsync(year, month);

            var result = monthlyStats.ToDictionary(
                kvp => kvp.Key,
                kvp => new
                {
                    TotalMinutes = kvp.Value.TotalMinutes,
                    Status = kvp.Value.Status,
                    StatusColor = kvp.Value.StatusColor
                }
            );
            return Ok(result);
        }
    }
}
