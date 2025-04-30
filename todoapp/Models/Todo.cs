using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace todoapp.Models
{
    // Represents a task in the ToDo application
    public class Todo
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public DateTime? DueDate { get; set; }
        public string? Priority { get; set; }
        public string? Tags { get; set; }
        public DateTime? Reminder { get; set; }
        public DateTime? CompletedDate { get; set; }
    }
}
