using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalentBookingManagement.Models
{
    public class Talent
    {
        public int TalentID { get; set; }
        public string AvailabilityStatus { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string Suburb { get; set; }
        public string StreetAddress { get; set; }
        public string Postcode { get; set; }
        public string Skill { get; set; }
        public int BookingID { get; set; }
        public DateTime BookingStartDate { get; set; }
        public DateTime BookingEndDate { get; set; }
        public string SpecialRequirement { get; set; }
        public string CampaignLocation { get; set; }
        public string PreferredEngagement { get; set; }
        public decimal HourlyRates { get; set; }
        public decimal DailyRates { get; set; }
    }

}
