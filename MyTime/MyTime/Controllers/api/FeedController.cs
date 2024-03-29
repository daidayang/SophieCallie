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
        public UsageControls Get(string id)
        {
            List<ControlItem> block_ctrls = null;
            List<ControlItem> nonblock_ctrls = null;

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
