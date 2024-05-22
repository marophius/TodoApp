using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Todo = TodoApp.Domain.Todo;

namespace TodoApp.Infrastructure.Configuration
{
    public class TodoConfiguration : IEntityTypeConfiguration<Todo>
    {
        public const int NAME_MAX_LENGTH = 30;
        public const int DESCRIPTION_MAX_LENGTH = 100;
        public void Configure(EntityTypeBuilder<Todo> builder)
        {
            builder.ToTable("Tasks");
            builder.HasKey(t => t.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(NAME_MAX_LENGTH);

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(DESCRIPTION_MAX_LENGTH);

            builder.Property(x => x.PrevisionDate)
                .IsRequired();

            builder.Property(x => x.IsCompleted)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.UpdatedAt)
                .IsRequired();

            builder.HasMany(x => x.SubTasks)
                .WithOne(t => t.Todo)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
