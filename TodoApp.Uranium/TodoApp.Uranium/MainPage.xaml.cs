using InputKit.Shared.Controls;
using TodoApp.Uranium.ViewModels;
using UraniumUI.Pages;

namespace TodoApp.Uranium
{
    public partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            var vm = (MainPageViewModel)BindingContext;
            vm.LoadTodos();
            base.OnAppearing();
        }
    }
}