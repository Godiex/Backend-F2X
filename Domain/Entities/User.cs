using Domain.Entities.Base;

namespace Domain.Entities;

public class User : EntityBase<Guid>
{
    public string Name { get; set; }
    public string Password { get; set; }
    
}