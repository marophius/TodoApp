using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Application.cs.Contracts;
using TodoApp.Domain;

namespace TodoApp.Infrastructure.Context
{
    public class TodoContext : DbContext
    {
        public DbSet<Todo> Tasks { get; set; }
        public DbSet<SubTask> SubTasks { get; set; }

        public TodoContext()
        {
            Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TodoContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            var databaseName = "apptask.db";
            var databasePath = Path.Combine(path, databaseName);
            optionsBuilder.UseSqlite($"Filename={databasePath}");
        }
    }
}
