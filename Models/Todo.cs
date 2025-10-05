namespace Taskly.Api.Models;

public class Todo
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime? DueDate { get; set; } // Dấu ? cho phép nó có thể null (không bắt buộc)
    public int Priority { get; set; } // 0 = Thấp, 1 = Trung bình, 2 = Cao
    // Foreign Key đến nhiệm vụ cha
    // Id của công việc cha. Nếu là null, đây là công việc gốc.
    public int? ParentId { get; set; }

    // Navigation Properties (Thuộc tính điều hướng)
    public virtual Todo? Parent { get; set; }
    public virtual ICollection<Todo> Subtasks { get; set; } = new List<Todo>();
}