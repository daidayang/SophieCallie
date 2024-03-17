using Microsoft.AspNetCore.Mvc;
using MyTime.Models;
using System.Diagnostics;
using MySql.Data.MySqlClient;

using MyTime.Models; // Use the correct namespace for your model

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
    }
}