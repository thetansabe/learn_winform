using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace todoapp.Views
{
    public partial class TodoView : Form, ITodoView
    {
        public TodoView()
        {
            InitializeComponent();

            // Event binding
            AssociateAndRaiseViewEvents();
        }

        private void AssociateAndRaiseViewEvents()
        {
            btnAdd.Click += (s, e) => AddTodo?.Invoke(this, EventArgs.Empty);
            btnEdit.Click += (s, e) => UpdateTodo?.Invoke(this, EventArgs.Empty);
            btnDelete.Click += (s, e) => DeleteTodo?.Invoke(this, EventArgs.Empty);
            btnSearch.Click += (s, e) => SearchTodo?.Invoke(this, EventArgs.Empty);
            btnExport.Click += (s, e) => ExportTodos?.Invoke(this, EventArgs.Empty);
        }

        public string TodoId { get; set; }
        public string TodoTitle { get; set; }
        public string TodoType { get; set; }
        public string TodoStatus { get; set; }
        public string TodoDescription { get; set; }
        public string SearchText { get; set; }
        public string Message { get; set; }

        public event EventHandler AddTodo;
        public event EventHandler UpdateTodo;
        public event EventHandler DeleteTodo;
        public event EventHandler SearchTodo;
        public event EventHandler ShowAllTodos;
        public event EventHandler ExportTodos;

        public void SetTodosBindingSource(BindingSource todoBindingSource)
        {
            dataGridView1.DataSource = todoBindingSource;
        }
    }
}
