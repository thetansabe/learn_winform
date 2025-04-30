using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using todoapp.Models;
using todoapp.Views;

namespace todoapp.Presenters
{
    public class TodoPresenter
    {
        private ITodoView _view;
        private ITodoRepository _repository;
        private BindingSource todoBindingSource;
        private List<Todo> todos;
        private string _filePath = ConfigurationManager.ConnectionStrings["Export"].ConnectionString;

        public TodoPresenter(ITodoView view, ITodoRepository repository)
        {
            _view = view;
            _repository = repository;
            todoBindingSource = new BindingSource();
            todos = new List<Todo>();

            // Subscribe to events
            _view.AddTodo += OnAddTodo;
            _view.UpdateTodo += OnUpdateTodo;
            _view.DeleteTodo += OnDeleteTodo;
            _view.SearchTodo += OnSearchTodo;
            _view.ShowAllTodos += OnShowAllTodos;

            // Subscribe to export event
            _view.ExportTodos += OnExportTodos;

            // Set the binding source for the view
            _view.SetTodosBindingSource(todoBindingSource);

            // Initialize the view
            LoadAllTodos();

            // Start the autosave feature
            //Task.Run(() => AutoSave());

            // Show the view
            _view.Show();
        }

        // Method to autosave after a period of inactivity
        private async void AutoSave()
        {
            while (true)
            {
                await Task.Delay(TimeSpan.FromSeconds(5)); // Autosave every 5 secs

                if (_view is Form viewForm && viewForm.InvokeRequired)
                {
                    viewForm.Invoke(new EventHandler(OnUpdateTodo), this, EventArgs.Empty);
                }
                else
                {
                    OnUpdateTodo(this, EventArgs.Empty);
                }
                LoadAllTodos(); // Reload after attempting to save
            }
        }

        // export todos to a csv, this must not block the UI thread
        // reflect the saving back to the progress bar
        private void OnExportTodos(object? sender, EventArgs e)
        {
            
        }

        private void LoadAllTodos()
        {
            // Check if the current thread is the UI thread
            if (_view is Form viewForm && viewForm.InvokeRequired)
            {
                // If not on the UI thread, invoke this method on the UI thread
                viewForm.Invoke(new MethodInvoker(LoadAllTodos));
                return;
            }

            // If on the UI thread, proceed with updating the binding source
            todos = _repository.GetAllTodos();
            todoBindingSource.DataSource = todos;
        }

        private void OnShowAllTodos(object? sender, EventArgs e)
        {
            var todo = (Todo)todoBindingSource.Current;
        }

        private void OnSearchTodo(object? sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnDeleteTodo(object? sender, EventArgs e)
        {
            var todo = (Todo) todoBindingSource.Current;
            if (todo != null)
            {
                _repository.DeleteTodo(todo.Id);
                LoadAllTodos();
            }
        }

        private void OnUpdateTodo(object? sender, EventArgs e)
        {
            var todo = (Todo) todoBindingSource.Current;
            _repository.UpdateTodo(todo);
            LoadAllTodos();
        }

        private void OnAddTodo(object? sender, EventArgs e)
        {
            var todo = (Todo) todoBindingSource.Current;
            _repository.AddTodo(todo);
            LoadAllTodos();
        }
    }
}
