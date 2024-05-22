namespace TodoApp.Uranium
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("home", typeof(MainPage));
            Routing.RegisterRoute("create-todo", typeof(Views.CreateTodoView));
            Routing.RegisterRoute("edit-todo", typeof(Views.EditTodoView));
        }
    }
}
