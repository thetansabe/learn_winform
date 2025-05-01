using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using todoapp.Models;

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

            // 2. làm sao để quá trình này không block UI
            // export csv
            btnExport.Click += (s, e) =>
            {
                if (!backgroundWorker.IsBusy)
                {
                    backgroundWorker.RunWorkerAsync();
                }
            };
            backgroundWorker.DoWork += (s, e) =>
            {
                // get todos from the data source
                var todoBindingSource = dataGridView1.DataSource as BindingSource;
                var todos = todoBindingSource?.DataSource as List<Todo>;
                // export to csv
                ExportTodosToCSV(todos);
            };
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.ProgressChanged += (s, e) =>
            {
                // Update progress bar or UI if needed
                progressBar1.Value = e.ProgressPercentage;
                lblPercent.Text = $"Proccessing ... {e.ProgressPercentage}%";
                progressBar1.Update();
            };
            backgroundWorker.RunWorkerCompleted += (s, e) =>
            {
                // Reset progress bar
                progressBar1.Value = 0;
                lblPercent.Text = "Export Completed!";
            };
            
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

        // draft code to export to csv
        public void ExportTodosToCSV(List<Todo> todos)
        {
            try
            {
                // a. Tại sao số > 1200 lại làm code chậm hơn nhiều lần
                for (int i = 0; i < 1200; i++)
                {
                    if (!backgroundWorker.CancellationPending)
                    {
                        backgroundWorker.ReportProgress((i + 1) * 100 / 1200);
                        // b. Nếu không deplay thì progress bar update không đúng nữa
                        Thread.Sleep(100); 
                    }

                }
                //if (todos != null)
                //{
                //    string filePath = ConfigurationManager.ConnectionStrings["Export"].ConnectionString;
                //    using (var writer = new StreamWriter(filePath))
                //    {
                //        // Write header
                //        writer.WriteLine("Id,Title,Status,Description");

                //        // Write each todo item
                //        for (int i = 0; i < todos.Count; i++)
                //        {
                //            // 3. làm sao để đưa cả hàm này vào presenter layer
                //            // khi mà chỉ có thể truy cập backgroundWorker ở view layer
                //            backgroundWorker.ReportProgress((i + 1) * 100 / todos.Count); // Report progress
                //            //writer.WriteLine($"{todos[i].Id},{todos[i].Title},{todos[i].Status},{todos[i].Description}");
                //            // 1. nếu enable dòng code dưới, làm sao mà background task ko update sự kiện done
                //            //Thread.Sleep(1000); // Simulate some delay for progress
                //        }
                //    }
                //    MessageBox.Show("Todos exported successfully!");
                //}
            }
            catch (Exception ex)
            {
                backgroundWorker.CancelAsync();
                MessageBox.Show($"Error exporting todos: {ex.Message}");
            }
        }

    }
}
