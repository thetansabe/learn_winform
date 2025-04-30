using System.Configuration;
using todoapp.Models;
using todoapp.Presenters;
using todoapp.Repositories;
using todoapp.Views;

namespace todoapp
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            
            // MVP
            ITodoView view = new TodoView();
            ITodoRepository repository = new TodoRepository(ConfigurationManager.ConnectionStrings["Sqlite3"].ConnectionString);
            new TodoPresenter(view, repository);

            Application.Run((Form) view);
        }
    }
}