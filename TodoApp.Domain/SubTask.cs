using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Domain.DomainObjects;

namespace TodoApp.Domain
{
    public class SubTask
    {
        protected SubTask() { }
        public SubTask(
            string name,
            Guid todoId,
            bool isCompleted)
        {
            Id = Guid.NewGuid();
            Name = name;
            IsCompleted = isCompleted;
            TodoId = todoId; 
        }
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public bool IsCompleted { get; private set; }
        public Guid TodoId { get; private set; }
        public Todo Todo { get; private set; }

        public void ChangeStatus()
        {
            IsCompleted = !IsCompleted;
        }
    }
}
