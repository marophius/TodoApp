using Plugin.LocalNotification;
using System.Reactive.Linq;
using TodoApp.Application.cs.Todos;
using TodoApp.Application.cs.Todos.DTOs;
using TodoApp.ViewModels;

namespace TodoApp
{
    public partial class MainPage : ContentPage
    {
        public ITodoService _todoService;

        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            var vm = (MainPaigeViewModel)BindingContext;
            vm.LoadTodos();
            
            base.OnAppearing();
        }

        private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
        {
            Entry_Search.Focus();
        }

        //private async void ChangeStatus(object sender, TappedEventArgs e)
        //{
        //    var task = (TodoResponse)e.Parameter;
        //    task = task with { Status = !task.Status };

        //    var vm = (MainPaigeViewModel)BindingContext;
        //    vm.TapToChangeStatusCommand.Execute(task);
        //}
    }

}
