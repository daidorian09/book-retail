using Domain.Common;

namespace Domain.Entities;

public class Customer : BaseEntity
{
    public  string FirstName { get; set; }
    public  string LastName { get; set; }
    public  string Address { get; set; }
    public  string Email { get; set; }
}
