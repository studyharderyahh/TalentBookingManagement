using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalentBookingManagement
{
    public class Booking
    {
        public int BookingID { get; set; }
        public string SpecialRequirement { get; set; }
        public int ClientID { get; set; }
        public int CampaignID { get; set; }
        public int StaffID { get; set; }
        public DateTime BookingTime { get; set; }
        public DateTime CampaignStartDate { get; set; }
        public DateTime CampaignEndDate { get; set; }
        public string CampaignLocation { get; set; }
        public string ActiveStatus { get; set; }
    }
}
