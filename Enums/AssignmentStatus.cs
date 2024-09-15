// Namespace donde se definen los enumeradores que controlan estados y prioridades de las tareas (Assignments)
namespace MyBackendNemura.Enums;

// Enumerador que define los posibles estados de una tarea
public enum AssignmentStatus
{
    // Tarea pendiente por hacer
    ToDo = 0,

    // Tarea en proceso
    Doing = 1,

    // Tarea completada
    Done = 2
}
