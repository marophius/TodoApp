using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Application.cs.Todos.DTOs;
using TodoApp.Domain;

namespace TodoApp.Application.cs.Todos.Mapper
{
    public static class TodoMapper
    {
        public static TodoResponse ToTodoResponse(this Todo todo)
        {
            return new TodoResponse(
                todo.Id, 
                todo.Name, 
                todo.Description, 
                todo.IsCompleted, 
                todo.PrevisionDate, 
                todo.CreatedAt, 
                todo.UpdatedAt, 
                todo.SubTasks.Select(t => t.ToSubTaskDTO()).ToList());
        }

        public static Todo ToTodo(this CreateTodoDTO dto)
        {
            return new Todo(dto.Name, dto.Description, DateOnly.FromDateTime(dto.PrevisionDate));
        }

        public static SubTask ToSubtask(this SubTaskDTO sub, Guid todoId)
        {
            return new SubTask(sub.Name, todoId, sub.Status);
        }

        public static SubTaskDTO ToSubTaskDTO(this SubTask sub)
        {
            return new SubTaskDTO(sub.Id, sub.Name, sub.IsCompleted, sub.TodoId);
        }
    }
}
