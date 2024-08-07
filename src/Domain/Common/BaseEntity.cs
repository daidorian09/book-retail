namespace Domain.Common;
public abstract class BaseEntity
{
    public Guid Id { get; set; }
    public long CreatedDate { get; set; }
    public string? CreatedBy { get; set; }
    public long? LastModifiedDate { get; set; }
    public string? LastModifiedBy { get; set; }
}
