using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTime.Models.api
{
    [Serializable]
    public class UsageControls
    {
        public List<UsageControl> Controls { get; set; }
    }
}
