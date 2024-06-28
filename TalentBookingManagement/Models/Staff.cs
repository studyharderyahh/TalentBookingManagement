using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalentBookingManagement.Models
{
    public class Staff : Person
    {
        public int StaffID { get; set; }
        public string Username { get; set; }                
        public string Password { get; set; }
        public int RoleID { get; set; }
    }
}
