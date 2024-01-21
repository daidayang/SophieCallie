using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.BusinessEntities
{
   public class MyBbe
    {
        public int BrandingID { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public bool Default { get; set; }
        public bool ChainBranding { get; set; }

        public string PreviewUrl { get; set; }
    }
}
