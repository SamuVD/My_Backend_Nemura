using MyBackendNemura.Enums;

namespace MyBackendNemura.Dtos.Assignment;

public class AssignmentGetDto
{
    public int Id {get; set;}
    public string Name {get; set;}
    public string Description {get; set;}
    public AssignmentStatus Status {get; set;}
    public AssignmentPriority Priority {get; set;}
    public int ProjectId {get; set;}
}
