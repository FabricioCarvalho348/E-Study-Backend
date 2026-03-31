using EStudy.Domain.Entities.Base;

namespace EStudy.Domain.Entities;

public class UserTask : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public bool IsCompleted { get; set; }
    public long UserId { get; set; }
    public User User { get; set; } = default!;
}

