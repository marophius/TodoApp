using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.Application.cs.Todos.DTOs
{
    public record EditTodoDTO(
        Guid Id,
        string Name,
        string Description,
        DateTime PrevisionDate,
        bool Status
        );
}
