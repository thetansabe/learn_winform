using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
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
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Debug.WriteLine("Current thread id: " + Thread.CurrentThread.ManagedThreadId);
        }

        private void AssociateAndRaiseViewEvents()
        {
            btnAdd.Click += (s, e) => AddTodo?.Invoke(this, EventArgs.Empty);
            btnEdit.Click += (s, e) => UpdateTodo?.Invoke(this, EventArgs.Empty);
            btnDelete.Click += (s, e) => DeleteTodo?.Invoke(this, EventArgs.Empty);
            btnSearch.Click += (s, e) => SearchTodo?.Invoke(this, EventArgs.Empty);
            btnSearch.Click += (s, e) => CustomEvent?.Invoke(this, textBox1.Text);

            // export csv
            btnExport.Click += (s, e) =>
            {
                if(!backgroundWorker.IsBusy)
                {
                    backgroundWorker.RunWorkerAsync();
                }
                else
                {
                    MessageBox.Show("Export is already in progress. Please wait.");
                }
            };

            backgroundWorker.DoWork += (s, e) =>
            {
                ExportTodos?.Invoke(this, EventArgs.Empty);
            };  
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.WorkerSupportsCancellation = true;
            //backgroundWorker.ProgressChanged += (s, e) =>
            //{
            //    // Update progress bar or UI if needed
            //    progressBar1.Value = e.ProgressPercentage;
            //    lblPercent.Text = $"Proccessing ... {e.ProgressPercentage}%";
            //    progressBar1.Update();
            //};
            //backgroundWorker.RunWorkerCompleted += (s, e) =>
            //{
            //    // Reset progress bar
            //    progressBar1.Value = 0;
            //    lblPercent.Text = "Export Completed!";
            //};
            
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
        public event EventHandler<string> CustomEvent;

        public void ReportProgress(int progressPercentage)
        {
            if(backgroundWorker.CancellationPending)
            {
                return;
            }
            backgroundWorker.ReportProgress(progressPercentage);
        }

        public void SetTodosBindingSource(BindingSource todoBindingSource)
        {
            dataGridView1.DataSource = todoBindingSource;
        }

        // draft code to export to csv
        //public void ExportTodosToCSV(List<Todo> todos)
        //{
        //    try
        //    {
        //        // a. Tại sao số > 1200 lại làm code chậm hơn nhiều lần
        //        // số quá lớn còn làm cho UI bị lag
        //        for (int i = 0; i < 1200000; i++)
        //        {
        //            if (!backgroundWorker.CancellationPending)
        //            {
        //                var a = (i + 1) * 100 / 1200000;
        //                Debug.WriteLine($"Exporting {a}% - Thread id {Thread.CurrentThread.ManagedThreadId}");
        //                backgroundWorker.ReportProgress(a);
        //                Thread.Sleep(100); // on thread hien tai
        //            }

        //        }
        //        //if (todos != null)
        //        //{
        //        //    string filePath = ConfigurationManager.ConnectionStrings["Export"].ConnectionString;
        //        //    using (var writer = new StreamWriter(filePath))
        //        //    {
        //        //        // Write header
        //        //        writer.WriteLine("Id,Title,Status,Description");

        //        //        // Write each todo item
        //        //        for (int i = 0; i < todos.Count; i++)
        //        //        {
        //        //            // 3. làm sao để đưa cả hàm này vào presenter layer
        //        //            // khi mà chỉ có thể truy cập backgroundWorker ở view layer
        //        //            backgroundWorker.ReportProgress((i + 1) * 100 / todos.Count); // Report progress
        //        //            writer.WriteLine($"{todos[i].Id},{todos[i].Title},{todos[i].Status},{todos[i].Description}");
        //        //            Thread.Sleep(1000); // Simulate some delay for progress
        //        //        }
        //        //    }
        //        //    MessageBox.Show("Todos exported successfully!");
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        backgroundWorker.CancelAsync();
        //        MessageBox.Show($"Error exporting todos: {ex.Message}");
        //    }
        //}

    }
}
