using TodoApp.Uranium.ViewModels;

namespace TodoApp.Uranium.Views;

public partial class EditTodoView : ContentPage
{
	public EditTodoView()
	{
		InitializeComponent();
	}

    protected override async void OnAppearing()
    {
        var vm = (EditTodoViewModel)BindingContext;
        var todo = vm.EditTodoDTO;
        await vm.GetSubTasks(todo.Id);
        base.OnAppearing();
    }
}