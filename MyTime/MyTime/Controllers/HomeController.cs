using Microsoft.AspNetCore.Mvc;
using MyTime.Models;
using System.Diagnostics;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using Microsoft.AspNetCore.Http;

using MyTime.Models; // Use the correct namespace for your model

namespace MyTime.Controllers
{
    public class HomeController : Controller
    {
        private string connectionString = "server=mytimedb;database=MyTime;uid=mytime;pwd=mytime123;";

        private List<TimeLeft> tasks = new List<TimeLeft>
    {
        new TimeLeft { Name = "Task 1", TimeLeftInMin = 90, State = true },
        new TimeLeft { Name = "Task 2", TimeLeftInMin = 60, State = false },
        // Add more tasks as needed
    };


        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var model = new TimeLeftTaskView
            {
                Tasks = tasks,
                SelectedTaskId = tasks[0].Name
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(string selectedTaskId, string action)
        {
            var model = new TimeLeftTaskView
            {
                Tasks = tasks,
                SelectedTaskId = selectedTaskId
            };

            if (action == "toggleState")
            {
                var selectedTask = tasks.FirstOrDefault(t => t.Name == selectedTaskId);
                if (selectedTask != null)
                {
                    selectedTask.State = !selectedTask.State;
                }
            }

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserLogin user)
        {
            // For demonstration, using hardcoded credentials
            // In a real application, you should validate against a database or other storage
            if (IsValidUser(user))
            {
                return RedirectToAction("Index"); // Redirect to the main page after successful login
            }
            else
            {
                ViewBag.LoginFailed = true;
                return View(user);
            }
        }

        public ActionResult SelectObject()
        {
            List<TimeLeft> objects = new List<TimeLeft>
            {
                new TimeLeft { Name = "Games", TimeLeftInMin = 30 },
                new TimeLeft { Name = "Videos", TimeLeftInMin = 45 }
            };

            ViewBag.ObjectList = objects;
            return View();
        }


        private bool IsValidUser(UserLogin user)
        {
            using var connection = new MySqlConnection(connectionString);
            connection.Open();
            var query = "SELECT UserID FROM Users WHERE Username = @Username AND Password = @Password";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Username", user.Username);
                command.Parameters.AddWithValue("@Password", user.Password); // In real applications, use hashed passwords

                var result = command.ExecuteScalar();
                if (result == null)
                    return false;

                HttpContext.Session.SetInt32("UserID", (int)result);
                int? value = HttpContext.Session.GetInt32("UserID");
                return true;
            }
        }
    }
}