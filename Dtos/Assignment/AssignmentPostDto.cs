using MyBackendNemura.Enums;

namespace MyBackendNemura.Dtos.Assignment;

public class AssignmentPostDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
    public string Priority { get; set; }
    public int ProjectId { get; set; }
}
