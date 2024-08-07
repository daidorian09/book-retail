namespace Domain.Common;
public abstract class BaseEntity
{
    public Guid Id { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset? LastModifiedDate { get; set; }
    public string? LastModifiedBy { get; set; }
}
