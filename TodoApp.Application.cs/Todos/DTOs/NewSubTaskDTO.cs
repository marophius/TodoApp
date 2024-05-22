using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.Application.cs.Todos.DTOs
{
    public record NewSubTaskDTO(string Name, bool Status);
}
