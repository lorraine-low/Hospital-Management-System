using System.Collections.Generic;

namespace Hospital_Management_System.Models
{
    public class Admins
    {
        public int AdminID { get; set; }
        public string Password { get; set; }
        public AccountType AccountType { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

        public Admins()
        {
            AccountType = AccountType.Admin;
        }

        public static List<Admins> GetAllAdmins()
        {
            List<Admins> admins = new List<Admins>
        {
            new Admins { AdminID = 10000001, Password = "Admin@123", FirstName = "Admin", Email = "admin1@sjwc.org.uk", Role = "Main Administrator" }
        };
            return admins;
        }
    }
}