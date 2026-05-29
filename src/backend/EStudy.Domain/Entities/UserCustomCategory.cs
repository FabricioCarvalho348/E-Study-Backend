using EStudy.Domain.Entities.Base;

namespace EStudy.Domain.Entities;

public class UserCustomCategory : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public long UserId { get; set; }
    public User User { get; set; } = default!;
    public IList<UserTask> UserTasks { get; set; } = new List<UserTask>();
}

