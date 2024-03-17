using Microsoft.AspNetCore.Mvc;
using MyTime.Models;
using System.Diagnostics;
using MySql.Data.MySqlClient;

namespace MyTime.Controllers
{
    public class FeedController : Controller
    {
        private string connectionString = "server=yourserver;database=yourdatabase;uid=yourusername;pwd=yourpassword;";
        private readonly ILogger<HomeController> _logger;

        public FeedController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
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

            return View();
        }
    }
}