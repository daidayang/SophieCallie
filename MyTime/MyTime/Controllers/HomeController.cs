using Microsoft.AspNetCore.Mvc;
using MyTime.Models;
using System.Diagnostics;
using MySql.Data.MySqlClient;

namespace MyTime.Controllers
{
    public class HomeController : Controller
    {
        private string connectionString = "server=yourserver;database=yourdatabase;uid=yourusername;pwd=yourpassword;";

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
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
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT COUNT(1) FROM Logins WHERE Username = @Username AND Password = @Password";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", user.Username);
                    command.Parameters.AddWithValue("@Password", user.Password); // In real applications, use hashed passwords

                    var result = (long)command.ExecuteScalar();
                    return result > 0;
                }
            }
        }
    }
}