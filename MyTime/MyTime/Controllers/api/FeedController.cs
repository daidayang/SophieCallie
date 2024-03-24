using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using MySql.Data.MySqlClient;

using MyTime.Models.api;
using System.Text.Json;
using Newtonsoft.Json;

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
            UsageControl uc = new()
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

                ControlItems = new List<ControlItem>
                {
                    new()
                    {
                        Identifier = "now.gg",
                        Type = ControlItemType.WWW
                    }
                }
            };

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
