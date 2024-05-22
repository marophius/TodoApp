using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Application.cs.Contracts;
using TodoApp.Domain;
using TodoApp.Infrastructure.Context;

namespace TodoApp.Infrastructure.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly TodoContext _context;
        
        public TodoRepository(TodoContext context)
        {
            _context = context;
        }
        public async Task<bool> Add(Todo task)
        {
            await _context.Tasks.AddAsync(task);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> AddSubTask(SubTask step)
        {
            await _context.SubTasks.AddAsync(step);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Delete(Todo task) =>
            await _context
                        .Tasks
                        .Where(t => t.Id == task.Id)
                        .ExecuteDeleteAsync() > 0;

        public async Task<bool> DeleteSubTask(string name, Guid todoId) =>
            await _context
                        .SubTasks
                        .Where (t => t.TodoId == todoId && t.Name == name)
                        .ExecuteDeleteAsync() > 0;

        public async Task<bool> EditSubTask(SubTask step)
        {
            _context.SubTasks.Update(step);
            return await _context.SaveChangesAsync() > 0;
        }

        public IAsyncEnumerable<Todo> GetAll() =>
             _context
                  .Tasks
                  .AsNoTracking()
                  .OrderBy(t => t.PrevisionDate)
                  .AsAsyncEnumerable();

        public IAsyncEnumerable<SubTask> GetSubtasksByTodoId(Guid todoId) =>
            _context
                    .SubTasks
                    .AsNoTracking()
                    .Where(sub => sub.TodoId == todoId)
                    .AsAsyncEnumerable();

        public async Task<Todo> GetTodo(Guid Id) =>
            await _context
                        .Tasks
                        .AsNoTracking()
                        .Include(t => t.SubTasks)
                        .FirstOrDefaultAsync(t => t.Id == Id);

        public async Task<bool> Update(Todo task)
        {
            var trackedTodo = _context.Tasks.Local.FirstOrDefault(t => t.Id == task.Id);
            if (trackedTodo != null)
            {
                _context.Entry(trackedTodo).CurrentValues.SetValues(task);
            }
            else
            {
                _context.Tasks.Update(task);
            }
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
