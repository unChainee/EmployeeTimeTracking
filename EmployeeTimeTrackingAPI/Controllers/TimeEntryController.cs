using EmployeeTimeTrackingAPI.Models;
using EmployeeTimeTrackingAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeTimeTrackingAPI.Controllers
{
    [Route("api/employees/{employeeId}/time-entries")]
    [ApiController]
    public class TimeEntryController : ControllerBase
    {
        private readonly ITimeEntryRepository _repository;

        public TimeEntryController(ITimeEntryRepository repository)
        {
            _repository = repository;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetByEmployeeId(int employeeId)
        {
            var timeEntries = await _repository.GetByEmployeeIdAsync(employeeId);
            return Ok(timeEntries);
        }

        [Authorize]
        [HttpGet("{timeEntryId}")]
        public async Task<IActionResult> GetById(int employeeId, int timeEntryId)
        {
            var timeEntry = await _repository.GetByIdAsync(employeeId, timeEntryId);
            return timeEntry is not null ? Ok(timeEntry) : NotFound();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(int employeeId, [FromBody] TimeEntry timeEntry)
        {
            // Walidacja: Nie może być dwóch wpisów w tej samej dacie
            var existing = (await _repository.GetByEmployeeIdAsync(employeeId))
                            .FirstOrDefault(te => te.Date.Date == timeEntry.Date.Date);
            if (existing != null)
            {
                return BadRequest("Wpis na ten dzień już istnieje.");
            }

            timeEntry.EmployeeId = employeeId;
            var id = await _repository.AddAsync(timeEntry);
            return CreatedAtAction(nameof(GetById), new { employeeId, timeEntryId = id }, timeEntry);
        }

        [Authorize]
        [HttpPut("{timeEntryId}")]
        public async Task<IActionResult> Update(int employeeId, int timeEntryId, [FromBody] TimeEntry timeEntry)
        {
            timeEntry.Id = timeEntryId;
            timeEntry.EmployeeId = employeeId;
            return await _repository.UpdateAsync(timeEntry) ? NoContent() : NotFound();
        }

        [Authorize]
        [HttpDelete("{timeEntryId}")]
        public async Task<IActionResult> Delete(int employeeId, int timeEntryId)
        {
            return await _repository.DeleteAsync(employeeId, timeEntryId) ? NoContent() : NotFound();
        }
    }
}
