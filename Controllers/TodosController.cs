using Microsoft.AspNetCore.Mvc;
using Taskly.Api.Models;
using Taskly.Api.Services;
using Microsoft.EntityFrameworkCore;

namespace Taskly.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodosController : ControllerBase
{
    private readonly TodoService _todoService;

    public TodosController(TodoService todoService)
    {
        _todoService = todoService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Todo>>> GetAll()
        => await _todoService.GetAllAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<Todo>> GetById(int id)
    {
        var todo = await _todoService.GetByIdAsync(id);
        if (todo == null) return NotFound();
        return todo;
    }

    [HttpPost]
    public async Task<ActionResult<Todo>> Create(Todo todo)
    {
        todo.ParentId = null;
        var newTodo = await _todoService.AddAsync(todo);
        return CreatedAtAction(nameof(GetById), new { id = newTodo.Id }, newTodo);
    }

    [HttpPost("{parentId}/subtasks")]
    public async Task<ActionResult<Todo>> CreateSubtask(int parentId, Todo subtask)
    {
        var newSubtask = await _todoService.AddSubtaskAsync(parentId, subtask);
        if (newSubtask == null)
        {
            return NotFound("Parent Todo not found.");  
        }
        return CreatedAtAction(nameof(GetById), new {id = newSubtask.Id}, newSubtask);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Todo todo)
    {
        if (id != todo.Id)
        {
            return BadRequest();
        }

        await _todoService.UpdateAsync(todo);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var todo = await _todoService.GetByIdAsync(id);
        if (todo == null) return NotFound();

        await _todoService.DeleteAsync(id);
        return NoContent();
    }
}