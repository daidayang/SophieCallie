using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsData.models
{
    public class UsageControl
    {
        public List<DateRange> DateRanges { get; set; }
        public List<ControlItem> ControlItems { get; set; }
    }
}
