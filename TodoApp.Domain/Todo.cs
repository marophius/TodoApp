using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using TodoApp.Domain.DomainObjects;

namespace TodoApp.Domain
{
    public class Todo
    {
        protected Todo() { }
        public Todo(
            string name,
            string description,
            DateOnly previsionDate
            )
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.Now;
            IsCompleted = false;

            UpdateTaskName(name);
            UpdateTaskDescription(description);
            UpdateTaskPrevisionDate(previsionDate);
        }
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public DateOnly PrevisionDate { get; private set; }
        public bool IsCompleted { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        private List<SubTask> _subtasks = new List<SubTask>(); 
        public IReadOnlyList<SubTask> SubTasks => _subtasks;

        public void UpdateTaskName(string name)
        {
            if(string.IsNullOrWhiteSpace(name) || name.Length < 5)
            {
                throw new DomainException("Name must have at least 5 characters");
            }
            Name = name;
            UpdatedAt = DateTime.Now;
        }

        public void UpdateTaskDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description) || description.Length < 5)
            {
                throw new DomainException("Name must have at least 5 characters");
            }
            Description = description;
            UpdatedAt = DateTime.Now;
        }

        public void UpdateTaskPrevisionDate(DateOnly previsionDate)
        {
            if(previsionDate < DateOnly.FromDateTime(DateTime.Now))
            {
                throw new DomainException("Invalid prevision date");
            }
            PrevisionDate = previsionDate;
            UpdatedAt = DateTime.Now;
        }

        public void AddSubTask(string name, bool isCompleted)
        {
            if(_subtasks.Exists(t => t.Name == name))
            {
                throw new DomainException($"{name} already exists in current list");
            }

            if(name.Length < 5)
            {
                throw new DomainException("Name must have at least 5 characters");
            }
            _subtasks.Add(new SubTask(name, Id, isCompleted));
            UpdatedAt = DateTime.Now;
        }

        public void UpdateSubTaskStatus(string name)
        {
            if (!_subtasks.Exists(t => t.Name == name))
            {
                throw new DomainException($"No subtask with name {name} found.");
            }

            var oldSub = _subtasks.FirstOrDefault(t => t.Name == name);
            oldSub.ChangeStatus();
            var newSub = oldSub;
            _subtasks.Remove(oldSub);
            _subtasks.Add(newSub);
            UpdatedAt = DateTime.Now;
        }

        public void RemoveSubTask(string name)
        {
            if(!_subtasks.Exists(t => t.Name == name))
            {
                throw new DomainException("No task found to remove");
            }

            var sub = _subtasks.FirstOrDefault(t => t.Name == name);
            _subtasks.Remove(sub);
            UpdatedAt = DateTime.Now;
        }

        public void UpdateStatus()
        {
            IsCompleted = !IsCompleted;
        }

    }
}
