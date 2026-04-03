using EStudy.Domain.Entities.Base;

namespace EStudy.Domain.Entities;

public class User : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public IList<UserTask> UserTasks { get; set; } = new List<UserTask>();
    public IList<Event> Events { get; set; } = new List<Event>();
    public Guid UserIdentifier { get; set; } = Guid.NewGuid();
}