using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.Application.cs.Todos.DTOs
{
    public record SubTaskDTO(
        Guid Id,
        string Name, 
        bool Status,
        Guid TodoId);
}
