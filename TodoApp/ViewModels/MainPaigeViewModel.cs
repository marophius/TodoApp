using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plugin.LocalNotification;
using Plugin.LocalNotification.AndroidOption;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Application.cs.Todos;
using TodoApp.Application.cs.Todos.DTOs;

namespace TodoApp.ViewModels
{
    public partial class MainPaigeViewModel : ObservableObject
    {
        private List<TodoResponse> _todos;

        [ObservableProperty]
        private IReadOnlyCollection<TodoResponse> _overdueTodoList;
        [ObservableProperty]
        private bool _overdueListIsVisible = false;
        [ObservableProperty]
        private IReadOnlyCollection<TodoResponse> _todayFilteredTodoList;
        [ObservableProperty]
        private bool _todayListIsVisible = true;
        [ObservableProperty]
        private IReadOnlyCollection<TodoResponse> _tomorrowFilteredTodoList;
        [ObservableProperty]
        private bool _tomorrowListIsVisible = true;
        [ObservableProperty]
        private IReadOnlyCollection<TodoResponse> _nextWeekFilteredTodoList;
        [ObservableProperty]
        private bool _nextWeekListIsVisible = true;
        private readonly ITodoService _todoService;
        [ObservableProperty]
        private string textSearch;
        private readonly INotificationService _notificationService;

        public MainPaigeViewModel()
        {
            _todoService = App.Current.Handler.MauiContext.Services.GetService<ITodoService>();
            _notificationService = App.Current.Handler.MauiContext.Services.GetService<INotificationService>();
        }

        [RelayCommand]
        private async Task OnTapCreateTodo()
        {
            Shell.Current.GoToAsync("create-todo");
        }

        [RelayCommand]
        private void TextChangedToSearch()
        {
            OverdueTodoList = _todos
                                        .Where(t => t.Name.ToLower().Contains(TextSearch.ToLower()) && t.PrevisionDate <= DateOnly.FromDateTime(DateTime.Now.AddDays(-1)))
                                        .ToList();
            OverdueListIsVisible = OverdueTodoList.Any();
            TodayFilteredTodoList = _todos
                                        .Where(t => t.Name.ToLower().Contains(TextSearch.ToLower()) && t.PrevisionDate == DateOnly.FromDateTime(DateTime.Now))
                                        .ToList();
            TodayListIsVisible = TodayFilteredTodoList.Any();
            TomorrowFilteredTodoList = _todos
                                        .Where(t => t.Name.ToLower().Contains(TextSearch.ToLower()) && t.PrevisionDate == DateOnly.FromDateTime(DateTime.Now.AddDays(1)))
                                        .ToList();
            TomorrowListIsVisible = TomorrowFilteredTodoList.Any();
            NextWeekFilteredTodoList = _todos
                                        .Where(t => t.Name.ToLower().Contains(TextSearch.ToLower()) && t.PrevisionDate >= DateOnly.FromDateTime(DateTime.Now.AddDays(7)) && t.PrevisionDate >= DateOnly.FromDateTime(DateTime.Now.AddDays(2)))
                                        .ToList();
            NextWeekListIsVisible = NextWeekFilteredTodoList.Any();
            if (string.IsNullOrEmpty(TextSearch))
            {
                TodayListIsVisible = true;
                TomorrowListIsVisible = true;
                NextWeekListIsVisible = true;
                OverdueListIsVisible = OverdueTodoList.Any();
            }
        }

        [RelayCommand]
        private async Task OnTapDeleteTodo(TodoResponse todo)
        {
            var confirm = await Shell.Current.DisplayAlert(
                "Delete item ?", 
                "This operation will delete this item, are you sure about that?", "Yes", "No");
            if (confirm)
            {
                bool result = await _todoService.DeleteTodo(todo.Id);
                if (result)
                {
                    _todos.Remove(todo);
                    UpdateLists();
                }
                //await LoadTodos();
            }
            
        }

        [RelayCommand]
        private async Task OnTapToChangeStatus(TodoResponse todo)
        {
            var existingTodo = _todos.FirstOrDefault(t => t.Id == todo.Id);
            if (existingTodo is not null)
            {
                var success = await _todoService.ChangeTodoStatus(todo.Id);
                if (success is null)
                {
                    await Shell.Current.DisplayAlert("Something got wrong!", "Can't change todo's status", "Ok");
                }
                var index = _todos.IndexOf(existingTodo);
                _todos[index] = success;
                UpdateLists();
                if (success.Status)
                {
                    var successToast = Toast.Make($"Task {todo.Name} marked as finished");
                    successToast.Show();
                    return;
                }
                var failToast = Toast.Make($"Task {todo.Name} marked as incomplete");
                failToast.Show();
                return;
            }
        }

        [RelayCommand]
        private async Task OnTapEditTodo(TodoResponse todo)
        {
            var editTodo = new EditTodoDTO(todo.Id, todo.Name, todo.Description, todo.PrevisionDate.ToDateTime(TimeOnly.MaxValue), todo.Status);
            var param = new Dictionary<string, object>()
            {
                {"todo", editTodo }
            };

            Shell.Current.GoToAsync("edit-todo", param);
        }

        public async Task LoadTodos()
        {
            _todos = new List<TodoResponse>();
            await foreach(var todo in _todoService.GetTodos())
            {
                _todos.Add(todo);
            }

            UpdateLists();
            await PushNotifications(OverdueTodoList.Count, TodayFilteredTodoList.Count);
        }

        private void UpdateLists()
        {
            OverdueTodoList = _todos
                                    .Where(t => t.PrevisionDate <= DateOnly.FromDateTime(DateTime.Now.AddDays(-1)) && !t.Status)
                                    .ToList();
            OverdueListIsVisible = OverdueTodoList.Any();
            TodayFilteredTodoList = _todos
                                        .Where(t => t.PrevisionDate == DateOnly.FromDateTime(DateTime.Now))
                                        .ToList();
            TomorrowFilteredTodoList = _todos
                                        .Where(t => t.PrevisionDate == DateOnly.FromDateTime(DateTime.Now.AddDays(1)))
                                        .ToList();
            NextWeekFilteredTodoList = _todos
                                        .Where(t => t.PrevisionDate >= DateOnly.FromDateTime(DateTime.Now.AddDays(7)) && t.PrevisionDate >= DateOnly.FromDateTime(DateTime.Now.AddDays(2)))
                                        .ToList();
        }

        private async Task PushNotifications(int overdueTaskCount, int todayTaskCount)
        {
            await PushOverdueTaskNotifications(overdueTaskCount);
            await PushTodayTaskNotifications(todayTaskCount);
        }

        private async Task PushOverdueTaskNotifications(int overdueTaskCount)
        {
            if (overdueTaskCount > 0)
            {
                var request = new NotificationRequest
                {
                    NotificationId = 1330,
                    Title = "TodoApp",
                    Subtitle = "TodoApp",
                    Description = $"You have {overdueTaskCount} overdue task(s).",
                    BadgeNumber = 42,
                    Schedule = new NotificationRequestSchedule
                    {
                        NotifyTime = DateTime.Now.AddSeconds(5),
                        NotifyRepeatInterval = TimeSpan.FromDays(1)
                    }
                };
                await _notificationService.Show(request);
            }
        }

        private async Task PushTodayTaskNotifications(int todayTaskCount)
        {
            if (todayTaskCount > 0)
            {
                var request = new NotificationRequest
                {
                    NotificationId = 1500,
                    Title = "TodoApp",
                    Subtitle = "TodoApp",
                    Description = $"You have {todayTaskCount} task(s) for today!",
                    BadgeNumber = 42,
                    Schedule = new NotificationRequestSchedule
                    {
                        NotifyTime = DateTime.Now.AddSeconds(5),
                        NotifyRepeatInterval = TimeSpan.FromDays(1)
                    }
                };
                await _notificationService.Show(request);
            }
        }
    }
}
