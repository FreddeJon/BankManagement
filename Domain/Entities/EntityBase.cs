namespace Domain.Entities;

public class EntityBase
{
    public string? CreatedBy { get; set; }
    public string? LastModifiedBy { get; set; }
    public DateTime? LastModifiedDate { get; set; }
}