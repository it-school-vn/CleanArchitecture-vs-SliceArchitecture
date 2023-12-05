namespace VerticleSlice.Application.Features.CreateToDo;

public record CreateToDoRequest(string Name, DateOnly DueDate)
{
}
