using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Application.cs.Contracts;
using TodoApp.Application.cs.Todos.DTOs;
using TodoApp.Application.cs.Todos.Mapper;
using TodoApp.Application.cs.Todos.Validators;
using TodoApp.Domain;

namespace TodoApp.Application.cs.Todos
{
    public class TodoService : ITodoService
    {
        private readonly ITodoRepository _repository;

        public TodoService(ITodoRepository repository)
        {
            _repository = repository;
        }

        public async Task<SubTaskDTO> AddSubTask(NewSubTaskDTO dto, Guid todoId)
        {
            var todo = await _repository.GetTodo(todoId);
            if (todo == null)
            {
                return default;
            }

            todo.AddSubTask(dto.Name, dto.Status);
            var subTaskDTO = todo.SubTasks.FirstOrDefault(s => s.Name == dto.Name);
            await _repository.AddSubTask(subTaskDTO);
            return subTaskDTO.ToSubTaskDTO();
        }

        public async Task<TodoResponse> ChangeTodoStatus(Guid id)
        {
            if (id == Guid.Empty)
                return default;
            var todo = await _repository.GetTodo(id);
            if (todo is null)
                return default;
            todo.UpdateStatus();
            await _repository.Update(todo);

            return todo.ToTodoResponse();
        }

        public async Task<TodoResponse> CreateTodo(CreateTodoDTO dto)
        {
            if (!ExecuteValidation(new CreateTodoDTOValidator(), dto))
                return default;

            var todo = dto.ToTodo();
            var list = dto.SubTasks.ToList();
            list.ForEach(sub =>
            {
                todo.AddSubTask(sub.Name, sub.Status);
            });
            await _repository.Add(todo);

            return todo.ToTodoResponse();
        }

        public async Task<bool> DeleteTodo(Guid id)
        {
            var todo = await _repository.GetTodo(id);
            if(todo is null)
            {
                return false;
            }

            return await _repository.Delete(todo);
        }

        public async Task<SubTaskDTO> EditSubTask(SubTaskDTO dto, Guid todoId)
        {
            var todo = await _repository.GetTodo(todoId);
            if (todo is null)
                return default;

            todo.UpdateSubTaskStatus(dto.Name);
            var subTaskDTO = todo.SubTasks.FirstOrDefault(s => s.Name == dto.Name);
            await _repository.EditSubTask(subTaskDTO);
            return subTaskDTO.ToSubTaskDTO();
        }

        public async Task<TodoResponse> EditTodo(EditTodoDTO dto)
        {
            var todo = await _repository.GetTodo(dto.Id);
            if (todo is null)
                return default;

            todo.UpdateTaskName(dto.Name);
            todo.UpdateTaskPrevisionDate(todo.PrevisionDate);
            todo.UpdateTaskDescription(dto.Description);

            await _repository.Update(todo);

            return todo.ToTodoResponse();
        }

        public async IAsyncEnumerable<SubTaskDTO> GetSubTasksByTodoId(Guid todoId)
        {
            await foreach (var sub in _repository.GetSubtasksByTodoId(todoId))
            {
                yield return sub.ToSubTaskDTO();
            }
        }

        public async IAsyncEnumerable<TodoResponse> GetTodos()
        {
            var todos = _repository.GetAll();
            await foreach (var todo in todos)
            {
                yield return todo.ToTodoResponse();
            }
        }

        public async Task<bool> RemoveSubTask(string name, Guid todoId)
        {
            var todo = await _repository.GetTodo(todoId);
            if(todo is null) 
                return false;

            todo.RemoveSubTask(name);
            await _repository.Update(todo);
            return await _repository.DeleteSubTask(name, todoId);
        }

        private bool ExecuteValidation<TV, TE> (TV validation, TE dto) where TV : AbstractValidator<TE> where TE : class
        {
            var validator = validation.Validate(dto);

            return validator.IsValid;
        }
    }
}
