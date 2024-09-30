using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using MySql.Data.MySqlClient;

using MyTime.Models.api;
using System.Text.Json;
using Newtonsoft.Json;
using MyTime.Models;
using System.Threading.Tasks;

using log4net;
using MySqlX.XDevAPI;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Extensions.Options;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyTime.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedController : ControllerBase
    {
        private readonly MySettings _mySettings;

        public FeedController(IOptions<MySettings> mySettings)
        {
            _mySettings = mySettings.Value;
        }

        private static readonly ILog log = LogManager.GetLogger(typeof(FeedController));
        private string connectionString = "server=mytimedb;database=MyTime;uid=mytime;pwd=mytime123;";

        //// GET: api/<FeedController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/<FeedController>/5
        [HttpGet("{id}/GetUsageCtrl")]
        public UsageControls GetUsageCtrl(string id)
        {
            List<ControlItem> block_ctrls = null;
            List<ControlItem> nonblock_ctrls = null;

            log.Debug("Get method called.");

            using var connection = new MySqlConnection(connectionString);
            connection.Open();

            int tmpUserID = 0;
            if (!int.TryParse(id, out tmpUserID))
            {
                using (var command = new MySqlCommand("usp_GetUserIDByUsername", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("arg_UserName", id);

                    using var result = command.ExecuteReader();
                    if (result.Read())
                    {
                        tmpUserID = result.GetInt32(0);
                    }
                }
            }

            if (tmpUserID > 0)
            {
                using (var command = new MySqlCommand("usp_GetPlayItems", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("arg_UserID", tmpUserID);

                    using var result = command.ExecuteReader();
                    while (result.Read())
                    {

                        int TimeLeft = result.GetInt32(4);
                        Boolean InPlay = result.GetBoolean(5);

                        ControlItem ci = new()
                        {
                            Type = (result.GetString(2) == "WWW") ? ControlItemType.WWW : ControlItemType.APP,
                            Identifier = result.GetString(3)
                        };

                        if (!InPlay || TimeLeft <= 0)
                        {
                            block_ctrls ??= new List<ControlItem>();
                            block_ctrls.Add(ci);
                        }
                        else
                        {
                            nonblock_ctrls ??= new List<ControlItem>();
                            nonblock_ctrls.Add(ci);
                        }
                    }
                }
            }

            UsageControls ret = new()
            {
                Controls = new List<UsageControl>()
            };

            UsageControl ucBlocked = null;
            if (block_ctrls != null)
            {
                ucBlocked = new()
                {
                    DateRanges = new List<DateRange>
                    {
                        new()
                        {
                            DOW = MyDow.Sunday | MyDow.Monday | MyDow.Tuesday | MyDow.Wednesday | MyDow.Thursday | MyDow.Friday | MyDow.Saturday,
                            TimeRanges = new List<TimeRange>
                            {
                                new()
                                {
                                    StartHour = 0,
                                    StartMin = 0,
                                    EndHour = 23,
                                    EndMin = 59
                                }
                            }
                        }
                    },
                    ControlItems = block_ctrls
                };
                ret.Controls.Add(ucBlocked);
            }

            UsageControl ucUnBlocked = null;
            if (nonblock_ctrls != null)
            {
                ucUnBlocked = new()
                {
                    DateRanges = new List<DateRange>
                    {
                        new()
                        {
                            DOW = MyDow.Sunday | MyDow.Monday | MyDow.Tuesday | MyDow.Wednesday | MyDow.Thursday | MyDow.Friday | MyDow.Saturday,
                            TimeRanges = new List<TimeRange>
                            {
                                new()
                                {
                                    StartHour = 0,
                                    StartMin = 0,
                                    EndHour = 0,
                                    EndMin = 0
                                }
                            }
                        }
                    },
                    ControlItems = nonblock_ctrls
                };
                ret.Controls.Add(ucUnBlocked);
            }

            return ret;
        }


        [HttpGet("{id}/GetMyTask")]
        public MyTaasks GetMyTask(string id)
        {
            log.Debug("Get method called.");

            MyTaasks ret = new();
            if (DateTime.Now.Hour > 16 && (id == "1" || id == "2"))
            {
                ret.Tasks =
                [
                    new MyTimeTaskItem { TaskName = "GetTaskList", Options = "moviesjoy" },
                    new MyTimeTaskItem { TaskName = "TakeScreenShot", Options = "youtube" }
                ];
            }
            else
            {
                ret.Tasks = [];
            }
            return ret;
        }


        [HttpPost("{id}/PostTaskList")]
        public void PostTaskList(string id, List<WindowsTaskItem> tasks)
        {
            log.Debug("PostTaskList method called.");

            if (tasks == null)
                return;

            foreach (WindowsTaskItem wti in tasks)
            {
                log.DebugFormat("User: {0}, TaskName: {1}, ExePath: {2}, PID: {3}, Status: {4}", id, wti.TaskName, wti.ExePath, wti.PID, wti.Status);
            }

            return;
        }

        [HttpPost("{id}/PostImage")]
        public async Task<IActionResult> PostImage(string id, IFormFile image)
        {
            if (image == null || image.Length == 0)
            {
                return BadRequest("No image uploaded.");
            }

            // Define the path to save the image
            var imagePath = Path.Combine(_mySettings.ImagePath, id);

            // Create directory if not exists
            if (!Directory.Exists(imagePath))
            {
                Directory.CreateDirectory(imagePath);
            }

            // Define the full path for the image
            var filePath = Path.Combine(imagePath, image.FileName);

            // Save the image
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            return Ok(new { Message = "Image uploaded successfully.", FilePath = filePath });
        }

        //// POST api/<FeedController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<FeedController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<FeedController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
