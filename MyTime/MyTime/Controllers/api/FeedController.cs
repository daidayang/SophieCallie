using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using MySql.Data.MySqlClient;

using MyTime.Models.api;
using System.Text.Json;
using Newtonsoft.Json;
using MyTime.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyTime.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedController : ControllerBase
    {
        private string connectionString = "server=mytimedb;database=MyTime;uid=mytime;pwd=mytime123;";

        //// GET: api/<FeedController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/<FeedController>/5
        [HttpGet("{id}")]
        public UsageControls Get(int id)
        {
            List<ControlItem> ctrls = null;

            using var connection = new MySqlConnection(connectionString);
            connection.Open();
            var query = "usp_GetPlayItems";

            using (var command = new MySqlCommand(query, connection))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("arg_UserID", id);

                using var result = command.ExecuteReader();
                while (result.Read())
                {

                    int TimeLeft = result.GetInt32(4);
                    Boolean InPlay = result.GetBoolean(5);

                    if (!InPlay || TimeLeft <= 0)
                    {
                        ctrls ??= new List<ControlItem>();
                        ControlItem ci = new()
                        {
                            Type = (result.GetString(2) == "WWW") ? ControlItemType.WWW : ControlItemType.APP,
                            Identifier = result.GetString(3)
                        };
                        ctrls.Add(ci);
                    }
                }
            }

            UsageControl uc = null;
            if (ctrls != null)
            {
                uc = new()
                {
                    DateRanges = new List<DateRange>
                    {
                        new()
                        {
                            DOW = MyDow.Monday | MyDow.Tuesday | MyDow.Wednesday | MyDow.Thursday | MyDow.Friday | MyDow.Saturday | MyDow.Sunday,
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
                    ControlItems = ctrls
                };
            }

            UsageControls ret = new()
            {
                Controls = new List<UsageControl>
                {
                    uc
                }
            };

            return ret;
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
