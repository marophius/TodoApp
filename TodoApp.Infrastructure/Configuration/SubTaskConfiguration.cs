using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Domain;

namespace TodoApp.Infrastructure.Configuration
{
    public class SubTaskConfiguration : IEntityTypeConfiguration<SubTask>
    {
        private const int NAME_MAX_LENGTH = 30; 
        public void Configure(EntityTypeBuilder<SubTask> builder)
        {
            builder.ToTable("SubTasks");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(NAME_MAX_LENGTH);

            builder.HasOne(x => x.Todo)
                .WithMany(x => x.SubTasks)
                .HasForeignKey(x => x.TodoId);
        }
    }
}
