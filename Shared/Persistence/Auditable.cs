namespace Auth.Infrastructure;

public class Auditable
{
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid CreatedBy { get; set; }
    public Guid LastUpdatedBy { get; set; }
    public Boolean IsDeleted { get; set; }
}