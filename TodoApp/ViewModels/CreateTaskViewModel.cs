using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Application.cs.Todos;
using TodoApp.Application.cs.Todos.DTOs;

namespace TodoApp.ViewModels
{
    public partial class CreateTaskViewModel : ObservableObject
    {
        [ObservableProperty]
        private CreateTodoDTO _todo;

        private readonly ITodoService _todoService;

        public CreateTaskViewModel()
        {
            _todoService = App.Current.Handler.MauiContext.Services.GetService<ITodoService>();
            _todo = new CreateTodoDTO("", "", DateTime.Now, new());
        }

        [RelayCommand]
        private async Task OnTapNavigateToHome()
        {
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        private async Task OnTapAddSubTask()
        {
            var stepName = await Shell.Current.DisplayPromptAsync("SubTask", "Write the name of your step:", "Add", "Cancel");
            if(!string.IsNullOrEmpty(stepName) && !Todo.SubTasks.Any(sub => sub.Name == stepName))
            {
                var subTask = new NewSubTaskDTO(stepName, false);
                Todo.SubTasks.Add(subTask);
            }
            
        }

        [RelayCommand]
        private async Task OnTapEditSubTask(NewSubTaskDTO subTaskDTO)
        {
            var oldSubtask = Todo.SubTasks.FirstOrDefault(sub => sub.Name == subTaskDTO.Name);
            if(oldSubtask is not null)
            {
                var index = Todo.SubTasks.IndexOf(oldSubtask);
                Todo.SubTasks[index] = subTaskDTO;
            }
        }

        [RelayCommand]
        private async Task OnTapDeleteSubTask(NewSubTaskDTO subTaskDTO)
        {
            bool exists = Todo.SubTasks.Any(sub => sub.Name == subTaskDTO.Name);
            if(exists)
            {
                Todo.SubTasks.Remove(subTaskDTO);
            }
        }

        [RelayCommand]
        private async Task OnTapToCreateTodo()
        {
            if(!ValidateTodo(Todo))
            {
                Shell.Current.DisplayAlert("Invalid Todo", "You must follow the rules to create a Todo", "Ok");
                return;
            }
            var todo = await _todoService.CreateTodo(Todo);

            if(todo is null)
            {
                var failToast = Toast.Make("Something got wrong, can't create Todo", ToastDuration.Long, 14);
                await failToast.Show();
                return;
            }

            var successToast = Toast.Make("Todo successfully created!", ToastDuration.Short, 14);
            await successToast.Show();
            await Shell.Current.GoToAsync("..");
        }

        private bool ValidateTodo(CreateTodoDTO dto)
        {
            if (dto.Name.Length < 5 || dto.Description.Length < 5 || dto.PrevisionDate.Date < DateTime.Now.Date)
            {
                return false;
            }

            return true;
        }
    }
}
