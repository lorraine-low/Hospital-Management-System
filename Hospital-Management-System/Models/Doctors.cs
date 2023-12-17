using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hospital_Management_System.Models
{
    public class Doctors
    {

        public int DoctorID { get; set; }
        public string Password { get; set; }
        public AccountType AccountType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime Dob { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public string Address { get; set; }

        [Required(ErrorMessage = "Please enter a Specialization.")]
        [RegularExpression(@"^[a-zA-Z\s\-]+$", ErrorMessage = "Specialization can contain only letters, spaces, and hyphen.")]
        public string Specialization { get; set; }

        public Doctors()
        {
            AccountType = AccountType.Doctor;
        }

        public static List<Doctors> doctors = new List<Doctors>
        {
            new Doctors { DoctorID = 20000001, Password = "Doctor1!", FirstName = "William", LastName = "Parker", Gender = "Male", Dob = new DateTime(1989, 3, 8), Email = "william.parker@sjwc.org.uk", PhoneNo = "01316667777", Address = "30 Hillside Terrace, Aberdeen AB1 2XY", Specialization = "General Medicine"},

            new Doctors { DoctorID = 20000002, Password = "Doctor2!", FirstName = "Emily", LastName = "Anderson", Gender = "Female", Dob = new DateTime(1985, 7, 15), Email = "emily.anderson@sjwc.org.uk", PhoneNo = "01412223333", Address = "15 Greenhill Street, Glasgow G1 3SL", Specialization = "Cardiology"},

            new Doctors { DoctorID = 20000003, Password = "Doctor3!", FirstName = "Alexander", LastName = "MacDonald", Gender = "Male", Dob = new DateTime(1976, 11, 22), Email = "alexander.macdonald@sjwc.org.uk", PhoneNo = "01555444333", Address = "42 Royal Crescent, Edinburgh EH7 5AB", Specialization = "Orthopedics"},

            new Doctors { DoctorID = 20000004, Password = "Doctor4!", FirstName = "Sophie", LastName = "Murray", Gender = "Female", Dob = new DateTime(1982, 5, 10), Email = "sophie.murray@sjwc.org.uk", PhoneNo = "01788777666", Address = "18 Riverside Drive, Dundee DD1 4NR", Specialization = "Dermatology"},

            new Doctors { DoctorID = 20000005, Password = "Doctor5!", FirstName = "Robert", LastName = "Fletcher", Gender = "Male", Dob = new DateTime(1990, 9, 5), Email = "robert.fletcher@sjwc.org.uk", PhoneNo = "01666777888", Address = "25 Castle Street, Inverness IV1 1RD", Specialization = "Pediatrics"}
        };


        public static List<Doctors> GetAllDoctors()
        {
            return doctors;
        }
    }

    public class DoctorViewModel
    {
        public int DoctorID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}