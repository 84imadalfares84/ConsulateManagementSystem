using Consulate.Application.Features.Employees.Commands;
using Consulate.Application.Features.Employees.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Consulate.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IMediator _mediator;

    public EmployeesController(IMediator mediator)
    {
        _mediator = mediator;
    }
    //[Authorize(Roles = "Admin")]
    [HttpPost("create")]
    public async Task<IActionResult> Create(
        CreateEmployeeCommand command)
    {
        var id = await _mediator.Send(command);

        return Ok(id);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var employee = await _mediator.Send(new GetEmployeeByIdQuery(id));
        return Ok(employee);
    }
    //[Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAll(
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10)
    {
        var result = await _mediator.Send(
            new GetAllEmployeesQuery(pageNumber, pageSize));

        return Ok(result);
    }
    //[Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        Guid id,
        UpdateEmployeeCommand command)
    {
        command.Id = id;

        var result = await _mediator.Send(command);

        return Ok(result);
    }
    //[Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteEmployeeCommand(id));
        return NoContent(); // 204 لا توجد محتويات
    }
}
