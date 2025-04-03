using EmployeeTimeTrackingAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/employees")]
[ApiController]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeRepository _repository;

    public EmployeeController(IEmployeeRepository repository)
    {
        _repository = repository;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var employees = await _repository.GetAllAsync();
        return Ok(employees);
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var employee = await _repository.GetByIdAsync(id);
        return employee is not null ? Ok(employee) : NotFound();
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Employee employee)
    {
        var id = await _repository.AddAsync(employee);
        return CreatedAtAction(nameof(GetById), new { id }, employee);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Employee employee)
    {
        employee.Id = id;
        return await _repository.UpdateAsync(employee) ? NoContent() : NotFound();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        return await _repository.DeleteAsync(id) ? NoContent() : NotFound();
    }
}
