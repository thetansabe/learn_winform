namespace todoapp.Views
{

    public interface ITodoView
    {
        // Properties for the Todo view
        string TodoId { get; set; }
        string TodoTitle { get; set; }
        string TodoType { get; set; }
        string TodoStatus { get; set; }
        string TodoDescription { get; set; }
        string SearchText { get; set; }
        string Message { get; set; }

        // Events for the Todo view
        event EventHandler AddTodo;
        event EventHandler UpdateTodo;
        event EventHandler DeleteTodo;
        event EventHandler SearchTodo;
        event EventHandler ShowAllTodos;
        event EventHandler ExportTodos;

        event EventHandler<string> CustomEvent;
        void ReportProgress(int progressPercentage);

        // Methods for the Todo view
        void SetTodosBindingSource(BindingSource bindingSource);
        void Show();
    }
}
