using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Application.cs.Todos.DTOs;

namespace TodoApp.Application.cs.Todos
{
    public interface ITodoService
    {
        Task<TodoResponse> CreateTodo(CreateTodoDTO dto);
        Task<TodoResponse> EditTodo(EditTodoDTO dto);
        Task<TodoResponse> ChangeTodoStatus(Guid id);
        Task<bool> DeleteTodo(Guid id);
        IAsyncEnumerable<TodoResponse> GetTodos(); 
        IAsyncEnumerable<SubTaskDTO> GetSubTasksByTodoId(Guid todoId);
        Task<SubTaskDTO> AddSubTask(NewSubTaskDTO dto, Guid todoId);
        Task<SubTaskDTO> EditSubTask(SubTaskDTO dto, Guid todoId);
        Task<bool> RemoveSubTask(string name, Guid todoId);
    }
}
