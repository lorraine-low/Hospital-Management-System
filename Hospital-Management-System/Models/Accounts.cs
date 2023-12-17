using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hospital_Management_System.Models
{
    public enum AccountType
    {
        Admin,
        Doctor,
        Patient
    }

    public class Accounts
    {
        public int AccountID { get; set; }

        [Required(ErrorMessage = "Please enter a Password.")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$", ErrorMessage = "Password must be between 8 and 15 characters and contain one uppercase letter, one lowercase letter, one digit, and one special character.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please select an Account Type.")]
        public AccountType AccountType { get; set; }

        [Required(ErrorMessage = "Please enter a First Name.")]
        [RegularExpression(@"^[a-zA-Z']+$", ErrorMessage = "First Name can contain only letters and apostrophe.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter a Last Name.")]
        [RegularExpression(@"^[a-zA-Z']+$", ErrorMessage = "Last Name can contain only letters and apostrophe.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please select a Gender.")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Please enter a Date of Birth.")]
        [DataType(DataType.Date)]
        public DateTime Dob { get; set; }

        [Required(ErrorMessage = "Please enter a valid E-mail Address.")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Please enter a valid E-mail Address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter a Phone Number.")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Please enter a valid 11-digit Phone Number.")]
        public string PhoneNo { get; set; }

        [Required(ErrorMessage = "Please enter an Address.")]
        [RegularExpression(@"^[a-zA-Z0-9\s,.-/#]+$", ErrorMessage = "Please enter a valid Address.")]
        public string Address { get; set; }

        public Admins Admin { get; set; }

        public Patients Patient { get; set; }

        public Doctors Doctor { get; set; }

        public static List<Accounts> accounts;

        public static List<Accounts> GetAllAccounts()
        {
            accounts = new List<Accounts>();

            List<Admins> admins = Admins.GetAllAdmins();
            foreach (var admin in admins)
            {
                accounts.Add(new Accounts
                {
                    AccountID = admin.AdminID,
                    Admin = admin,
                    Password = admin.Password,
                    AccountType = admin.AccountType,
                    FirstName = admin.FirstName,
                    Email = admin.Email
                });
            }

            List<Patients> patients = Patients.GetAllPatients();
            foreach (var patient in patients)
            {
                accounts.Add(new Accounts
                {
                    AccountID = patient.PatientID,
                    Patient = patient,
                    Password = patient.Password,
                    AccountType = patient.AccountType,
                    Email = patient.Email,
                    FirstName = patient.FirstName,
                    LastName = patient.LastName,
                    Gender = patient.Gender,
                    Dob = patient.Dob,
                    PhoneNo = patient.PhoneNo,
                    Address = patient.Address
                });
            }

            List<Doctors> doctors = Doctors.GetAllDoctors();
            foreach (var doctor in doctors)
            {
                accounts.Add(new Accounts
                {
                    AccountID = doctor.DoctorID,
                    Doctor = doctor,
                    Password = doctor.Password,
                    AccountType = doctor.AccountType,
                    Email = doctor.Email,
                    FirstName = doctor.FirstName,
                    LastName = doctor.LastName,
                    Gender = doctor.Gender,
                    Dob = doctor.Dob,
                    PhoneNo = doctor.PhoneNo,
                    Address = doctor.Address
                });
            }

            return accounts;
        }

    }
}