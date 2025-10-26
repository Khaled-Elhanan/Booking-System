using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystem.Application.Common.Utility
{
    public static class SD
    {
        public const string Role_Admin = "Admin";
        public const string Role_Customer = "Customer";

        public const string StatusPending = "Pending";
        public const string StatusApproved = "Approved";
        public const string StatusCheckIn = "CheckIn";
        public const string StatusComplete = "Complete";
        public const string StatusCancelled = "Cancelled";
        public const string StatusRefunded = "Refunded";
    }
}
