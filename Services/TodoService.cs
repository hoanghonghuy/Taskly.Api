using Microsoft.EntityFrameworkCore;
using Taskly.Api.Data;
using Taskly.Api.Models;

namespace Taskly.Api.Services;

public class TodoService
{
    private readonly TodoDbContext _context;

    public TodoService(TodoDbContext context)
    {
        _context = context;
    }

    public async Task<List<Todo>> GetAllAsync()
    {
        return await _context.Todos.Where(t => t.ParentId == null).Include(t => t.Subtasks).ToListAsync();
    }

    public async Task<Todo?> GetByIdAsync(int id)
    {
        return await _context.Todos.Include(t => t.Subtasks).FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<Todo> AddAsync(Todo newTodo)
    {
        _context.Todos.Add(newTodo);
        await _context.SaveChangesAsync(); // Lưu thay đổi vào database
        return newTodo;
    }
    public async Task<Todo?> AddSubtaskAsync(int parentId, Todo newSubtask)
    {
        var parentTodo = await GetByIdAsync(parentId);
        if (parentTodo == null)
        {
            return null;
        }

        newSubtask.ParentId = parentId; 
        _context.Todos.Add(newSubtask);
        await _context.SaveChangesAsync();

        return newSubtask;
    }
    public async Task UpdateAsync(Todo updatedTodo)
    {
        // Tìm thực thể đang được DbContext theo dõi
        var existingTodo = await _context.Todos.FindAsync(updatedTodo.Id);

        if (existingTodo is null)
        {
            return;
        }

        // Cập nhật các thuộc tính của thực thể đã được theo dõi
        existingTodo.Title = updatedTodo.Title;
        existingTodo.IsCompleted = updatedTodo.IsCompleted;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var todoToDelete = await GetByIdAsync(id);
        if (todoToDelete != null)
        {
            _context.Todos.Remove(todoToDelete);
            await _context.SaveChangesAsync();
        }
    }
}