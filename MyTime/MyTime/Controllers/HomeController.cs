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

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult Index()
        {
            int ? userid = HttpContext.Session.GetInt32("UserID");
            List<TimeLeft> tasks = GetFreeTime((int)userid);

            var model = new TimeLeftTaskView
            {
                Tasks = tasks,
                SelectedTaskId = tasks[0].TypeID.ToString()
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(string selectedTaskId, string action)
        {
            int? userid = HttpContext.Session.GetInt32("UserID");
            List<TimeLeft> tasks = GetFreeTime((int)userid);

            if (action == "toggleState")
            {
                var selectedTask = tasks.FirstOrDefault(t => t.TypeID.ToString() == selectedTaskId);
                if (selectedTask != null)
                {
                    SetRunState((int)userid, selectedTask.TypeID, !selectedTask.State);
                    // selectedTask.State = !selectedTask.State;
                    tasks = GetFreeTime((int)userid);
                }
            }

            var model = new TimeLeftTaskView
            {
                Tasks = tasks,
                SelectedTaskId = selectedTaskId
            };

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

        private List<TimeLeft> GetFreeTime(int userid)
        {
            List<TimeLeft> ret = null;

            using var connection = new MySqlConnection(connectionString);
            connection.Open();
            var query = "usp_GetTimeLeft";

            using (var command = new MySqlCommand(query, connection))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("arg_ID", userid);

                using var result = command.ExecuteReader();
                while (result.Read())
                {
                    ret ??= new List<TimeLeft>();

                    TimeLeft rl = new()
                    {
                        TypeID = result.GetInt32(0),
                        TypeName = result.GetString(1),
                        TimeLeftInMin = result.GetInt32(2),
                        State = result.GetBoolean(3)
                    };
                    ret.Add(rl);
                }
            }

            return ret;
        }

        private void SetRunState(int userid, int playTypeID, bool setToRun)
        {
            using var connection = new MySqlConnection(connectionString);
            connection.Open();
            var query = "usp_ToggleState";

            using (var command = new MySqlCommand(query, connection))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("arg_ID", userid);
                command.Parameters.AddWithValue("arg_PlayTypeID", playTypeID);
                command.Parameters.AddWithValue("arg_Play", setToRun);
                command.ExecuteNonQuery();
            }
        }
    }
}