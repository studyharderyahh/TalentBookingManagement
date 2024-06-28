using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalentBookingManagement.Models
{
    public class Talent : Person
    {
        public int TalentID { get; set; }
        public string Skill { get; set; }
        public int BookingID { get; set; }
        public DateTime BookingStartDate { get; set; }
        public DateTime BookingEndDate { get; set; }
        public string SpecialRequirement { get; set; }
        public string CampaignLocation { get; set; }
        public string PreferredEngagement { get; set; }
        public decimal HourlyRates { get; set; }
        public decimal DailyRates { get; set; }
        public bool IsSelected { get; set; }
        public string AvailabilityStatus { get; set; }
    }
}
