using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.BusinessEntities
{
    public enum UserTypeEnum
    {
        SuperAdminManager = 1,
        SuperAdmin=2,
        Admin=3,
        User=4,
        NoLogin = 5,
        UserReservations=6,
        SourcingRequestAdmin=7,
        SourcingRequestUser=8
    }
}
