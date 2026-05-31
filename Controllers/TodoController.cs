using Microsoft.AspNetCore.Mvc;
using MultiTrack.Models;

namespace MultiTrack.Controllers
{
    public class TodoController : Controller
    {
        // GEÇİCİ LİSTE (database yok, RAM'de)
        private static List<TodoItem> todos = new List<TodoItem>
        {
            new TodoItem { Id = 1, Title = "Matematik çalış", IsDone = false },
            new TodoItem { Id = 2, Title = "ASP.NET öğren", IsDone = true },
            new TodoItem { Id = 3, Title = "Spor yap", IsDone = false }
        };

        public IActionResult Index()
        {
            return View(todos);
        }

        [HttpPost]
        public IActionResult Add(string title)
        {
            if (!string.IsNullOrEmpty(title))
            {
                todos.Add(new TodoItem
                {
                    Id = todos.Count + 1,
                    Title = title,
                    IsDone = false
                });
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var todo = todos.FirstOrDefault(x => x.Id == id);

            if (todo != null)
            {
                todos.Remove(todo);
            }

            return RedirectToAction("Index");
        }
    }
}