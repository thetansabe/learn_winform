using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using todoapp.Models;

namespace todoapp.Repositories
{
    public class TodoRepository : BaseRepository, ITodoRepository
    {
        public TodoRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void AddTodo(Todo todo)
        {
            using var connection = new SQLiteConnection(connectionString);
            connection.Execute("INSERT INTO todo " +
                "(Id, Title, Description, Status, DueDate, Priority, Tags, Reminder, CompletedDate) " +
                "VALUES (@Id, @Title, @Description, @Status, @DueDate, @Priority, @Tags, @Reminder, @CompletedDate)", 
                todo);
        }

        public void UpdateTodo(Todo todo)
        {
            using var connection = new SQLiteConnection(connectionString);
            connection.Execute("UPDATE todo SET " +
                               "Title = @Title, Description = @Description, Status = @Status, DueDate = @DueDate, " +
                               "Priority = @Priority, Tags = @Tags, Reminder = @Reminder, CompletedDate = @CompletedDate " +
                               "WHERE Id = @Id", todo);
        }

        public void DeleteTodo(int id)
        {
            using var connection = new SQLiteConnection(connectionString);
            connection.Execute("DELETE FROM todo WHERE Id = @Id", new { Id = id });
        }

        public Todo? GetTodo(int id)
        {
            using var connection = new SQLiteConnection(connectionString);
            var output = connection.QuerySingleOrDefault<Todo>("SELECT * FROM todo WHERE Id = @Id", new { Id = id });
            return output;
        }

        public List<Todo> GetAllTodos()
        {
            using var connection = new SQLiteConnection(connectionString);
            var output = connection.Query<Todo>("SELECT * FROM todo");
            return output.ToList();
        }

    }
}
