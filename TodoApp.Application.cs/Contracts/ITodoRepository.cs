using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Domain;

namespace TodoApp.Application.cs.Contracts
{
    public interface ITodoRepository
    {
        Task<bool> Add(Todo task);
        Task<bool> Update(Todo task);
        Task<bool> Delete(Todo task);
        Task<bool> AddSubTask(SubTask step);
        Task<bool> EditSubTask(SubTask step);
        Task<bool> DeleteSubTask(string name, Guid todoId);
        IAsyncEnumerable<Todo> GetAll();
        IAsyncEnumerable<SubTask> GetSubtasksByTodoId(Guid todoId);
        Task<Todo> GetTodo(Guid Id);
    }
}
