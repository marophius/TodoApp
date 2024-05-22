using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.Application.cs.Todos.DTOs
{
    public record TodoResponse(
        Guid Id,
        string Name,
        string Description,
        bool Status,
        DateOnly PrevisionDate,
        DateTime CreatedAt,
        DateTime UpdatedAt,
        List<SubTaskDTO> SubTasks);
}
