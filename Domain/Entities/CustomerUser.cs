using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class CustomerUser
{
    [Key] public int Id { get; set; }
    public string UserId { get; set; }
    public int CustomerId { get; set; }
}
