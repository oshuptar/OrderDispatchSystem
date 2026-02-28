namespace Auth.Persistence;

public class Auditable
{
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    // TODO: extract the info about user manipulating the data
    //public Guid CreatedBy { get; set; }
    //public Guid LastUpdatedBy { get; set; }
    public Boolean IsDeleted { get; set; }
}