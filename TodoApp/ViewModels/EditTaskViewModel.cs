using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Application.cs.Todos;
using TodoApp.Application.cs.Todos.DTOs;
using TodoApp.Domain;

namespace TodoApp.ViewModels
{
    [QueryProperty(nameof(EditTodoDTO), "todo")]
    public partial class EditTaskViewModel : ObservableObject
    {
        [ObservableProperty]
        private EditTodoDTO _editTodoDTO;

        private List<SubTaskDTO> _existingSubTasks;
        
        [ObservableProperty]
        private List<SubTaskDTO> allSubTasks;

        private readonly ITodoService _service;

        public EditTaskViewModel()
        {
            _service = App.Current.Handler.MauiContext.Services.GetService<ITodoService>();
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
            if (!string.IsNullOrEmpty(stepName) && !_existingSubTasks.Any(sub => sub.Name == stepName))
            {
                var subTask = new NewSubTaskDTO(stepName, false);
                var success = await _service.AddSubTask(subTask, EditTodoDTO.Id);
                if(success is not null)
                {
                   _existingSubTasks.Add(success);
                    AllSubTasks = _existingSubTasks.ToList();
                }
                return;
            }

            await Shell.Current.DisplayAlert("Invalid name", $"There is already a sub task with name: {stepName}. Choose another name.", "Ok");
        }

        [RelayCommand]
        private async Task OnTapEditSubTask(SubTaskDTO subTaskDTO)
        {
            var oldSubtask = _existingSubTasks.FirstOrDefault(sub => sub.Name == subTaskDTO.Name);
            if (oldSubtask is not null)
            {
                var success = await _service.EditSubTask(subTaskDTO, EditTodoDTO.Id);
                if(success is not null)
                {
                    var index = _existingSubTasks.IndexOf(oldSubtask);
                    _existingSubTasks[index] = success;
                    AllSubTasks = _existingSubTasks.ToList();
                    return;
                }
                await Shell.Current.DisplayAlert("Error", "Something wrong happened.", "Cancel");
                return;
            }
        }

        [RelayCommand]
        private async Task OnTapDeleteSubTask(SubTaskDTO subTaskDTO)
        {
            bool exists = _existingSubTasks.Any(sub => sub.Name == subTaskDTO.Name);
            if (exists)
            {
                bool success = await _service.RemoveSubTask(subTaskDTO.Name, EditTodoDTO.Id);
                if (success)
                {
                    _existingSubTasks.Remove(subTaskDTO);
                    AllSubTasks = _existingSubTasks.ToList();
                }
                return;
            }
        }

        [RelayCommand]
        private async Task OnEditTodo()
        {
            if (!ValidateTodo(EditTodoDTO))
            {
                Shell.Current.DisplayAlert("Invalid Todo", "You must follow the rules to edit your Todo", "Ok");
                return;
            }
            var result = await _service.EditTodo(EditTodoDTO);

            if(result is null)
            {
                Shell.Current.DisplayAlert("Something went wrong!", "Can't edit todo", "Ok");
            }

            Shell.Current.GoToAsync("..");
        }

        public async Task GetSubTasks(Guid id)
        {
            _existingSubTasks = new List<SubTaskDTO>();
            await foreach(var sub in _service.GetSubTasksByTodoId(id))
            {
                _existingSubTasks.Add(sub);
            }
            AllSubTasks = _existingSubTasks.ToList();
        }

        private bool ValidateTodo(EditTodoDTO dto)
        {
            if (dto.Name.Length < 5 || dto.Description.Length < 5 || dto.PrevisionDate.Date < DateTime.Now.Date)
            {
                return false;
            }

            return true;
        }
    }
}
