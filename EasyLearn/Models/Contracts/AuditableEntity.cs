namespace EasyLearn.Models.Contracts;

public abstract class AuditableEntity : BaseEntity, IAuditablentity, ISoftDelete
{
    public string? CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public string? ModifiedBy { get; set; }
    public DateTime ModifiedOn { get; set; } = DateTime.Now;
    public string? DeletedBy { get; set; }
    public DateTime DeletedOn { get; set; }
    public bool IsDeleted { get; set; }
}
