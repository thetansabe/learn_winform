namespace todoapp.Models
{
    public interface ITodoRepository
    {
        void AddTodo(Todo todo);
        void UpdateTodo(Todo todo);
        void DeleteTodo(int id);
        Todo? GetTodo(int id);
        List<Todo> GetAllTodos();
    }
}
