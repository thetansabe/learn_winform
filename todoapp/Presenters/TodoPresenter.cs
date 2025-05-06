using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
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
            _view.CustomEvent += OnCustomEvent;

            // Subscribe to export event
            _view.ExportTodos += OnExportTodos;
            // Set the binding source for the view
            _view.SetTodosBindingSource(todoBindingSource);

            // Initialize the view
            LoadAllTodos();

            // Start the autosave feature
            Task.Run(() => AutoSave());

            // Show the view
            _view.Show();
        }

        private void OnCustomEvent(object? sender, string e)
        {
            Debug.WriteLine($"Custom event triggered with message: {e}");
        }

        // Method to autosave after a period of inactivity
        private async Task AutoSave()
        {
            while (true)
            {
                Debug.WriteLine("current thread before task delay: " + Thread.CurrentThread.ManagedThreadId);
                await Task.Delay(TimeSpan.FromSeconds(10)).ConfigureAwait(false);
                Debug.WriteLine("current thread after task delay: " + Thread.CurrentThread.ManagedThreadId);
                //OnUpdateTodo(this, EventArgs.Empty);
            }
        }

        // export todos to a csv, this must not block the UI thread
        // reflect the saving back to the progress bar
        private void OnExportTodos(object? sender, EventArgs e)
        {
            for (int i = 0; i < 1200000; i++)
            {
                var a = (i + 1) * 100 / 1200000;
                Debug.WriteLine($"Exporting {a}% - Thread id {Thread.CurrentThread.ManagedThreadId}");
                _view.ReportProgress(a);
                Thread.Sleep(100); // on thread hien 
            }
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
            return;
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
