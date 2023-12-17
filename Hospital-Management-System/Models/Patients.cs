using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hospital_Management_System.Models
{
    public class Patients
    {
        public int PatientID { get; set; }
        public string Password { get; set; }
        public AccountType AccountType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime Dob { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public string Address { get; set; }

        [Required(ErrorMessage = "Please select a Blood Group.")]
        public string BloodGroup { get; set; }

        public Patients()
        {
            AccountType = AccountType.Patient;
        }

        public static List<Patients> patients = new List<Patients>
        {
            new Patients { PatientID = 30000001, Password = "Patient1!", FirstName = "Emily", LastName = "Johnson", Gender = "Female", Dob = new DateTime(1997, 5, 12), Email = "emily97@gmail.com", PhoneNo = "07123456789", Address = "45 Rose Gardens, Glasgow G2 8AA", BloodGroup = "O+"},

            new Patients { PatientID = 30000002, Password = "Patient2!", FirstName = "James", LastName = "Smith", Gender = "Male", Dob = new DateTime(1980, 8, 25), Email = "jsmith@yahoo.co.uk", PhoneNo = "07890123456", Address = "22 Oak Street, London SW1A 1AA", BloodGroup = "A-"},

            new Patients { PatientID = 30000003, Password = "Patient3!", FirstName = "Sophie", LastName = "Brown", Gender = "Female", Dob = new DateTime(1995, 3, 18), Email = "sophiebrownn@hotmail.com", PhoneNo = "07987654321", Address = "10 Park Lane, Manchester M2 4WD", BloodGroup = "B+"},

            new Patients { PatientID = 30000004, Password = "Patient4!", FirstName = "Daniel", LastName = "Wilson", Gender = "Male", Dob = new DateTime(1972, 11, 8), Email = "daniel.wilson@gmail.com", PhoneNo = "07543210987", Address = "5 Primrose Avenue, Belfast BT1 1FF", BloodGroup = "AB-"},

            new Patients { PatientID = 30000005, Password = "Patient5!", FirstName = "Olivia", LastName = "Miller", Gender = "Female", Dob = new DateTime(1988, 6, 7), Email = "olivia88@gmail.com", PhoneNo = "07451234567", Address = "15 High Street, Cardiff CF10 1BB", BloodGroup = "O+"},

        };


        public static List<Patients> GetAllPatients()
        {
            return patients;
        }
    }

    public class PatientViewModel
    {
        public int PatientID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}