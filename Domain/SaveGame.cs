using System.ComponentModel.DataAnnotations;

namespace Domain;

public class SaveGame
{
    //Primary key
    public Guid Id { get; set; }
    
    [MaxLength(128)]
    public string SaveGameName { get; set; } = default!;
    
    [MaxLength(10240)]
    public string State { get; set; } = default!;
    
    public Guid ConfigurationId { get; set; }
    public Configuration? Configuration { get; set; }
    
    [MaxLength(128)]
    public string CreatedAtDateTime { get; set; } = default!;
}