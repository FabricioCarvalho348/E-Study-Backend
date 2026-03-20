using EStudy.Domain.Entities.Base;

namespace EStudy.Domain.Entities;

public class RefreshToken : BaseEntity
{
    public required string Value { get; set; } = string.Empty;
    public required long UserId { get; set; }
    public User User { get; set; } = default!;
}