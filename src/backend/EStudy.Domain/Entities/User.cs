using System.ComponentModel.DataAnnotations.Schema;
using EStudy.Domain.Entities.Base;

namespace EStudy.Domain.Entities;

public class User : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public Guid UserIdentifier { get; set; } 
}