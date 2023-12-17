/*
 * Class: AdminController
 * Author: Lorraine Low
 * Date: 30/07/2023
 * File: AdminController.cs
 * ------------------------
 * This file contains the definition and implementation of the AdminController class.
 * This class extends the Controller class provided by the ASP.NET MVC Framework. 
 * It is used to manage the administration operations of the system, including account creation, 
 * modification, and deletion, as well as assigning doctors to appointments.
 *
 * Major functionalities include:
 * - Displaying all the accounts and appointments (GET: Admin)
 * - Displaying the details of a specific account (GET: Admin Detail)
 * - Allowing the modification of account details (GET & POST: Admin Edit)
 * - Adding new accounts to the system (GET & POST: Admin Add)
 * - Assigning a doctor to an appointment (GET & POST: Admin Assign)
 * - Deleting an account (POST: Admin Delete)
 *
 * All operations first check if the current session belongs to an administrator.
 */

using Hospital_Management_System.Models;
using Hospital_Management_System.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;

namespace Hospital_Management_System.Controllers
{
    public class AdminController : Controller
    {

        // GET: Admin
        public ActionResult Index()
        {
            if (!IsAdmin())
            {
                return Redirect("../Login/index");
            }

            var accounts = LoadAccounts();
            var appointments = LoadAppointments();

            ViewBag.DoctorList = GetAccountType(accounts, AccountType.Doctor);
            ViewBag.PatientList = GetAccountType(accounts, AccountType.Patient);
            ViewBag.AppointmentList = GetAppointments(appointments);

            return View();
        }

        // GET: Admin Detail
        public ActionResult Detail(int ID)
        {
            if (!TryGetAccountDetails(ID, out var item))
                return RedirectToAction("Index", "Admin");

            SetAccountsViewBag(item);
            return View();
        }

        // GET: Admin Edit
        public ActionResult Edit(int ID)
        {
            InitializeDropDownLists();
            if (!TryGetAccountDetails(ID, out var item))
                return RedirectToAction("Index", "Admin");

            SetAccountsViewBag(item);
            return View(item);
        }

        // GET: Admin Add
        public ActionResult Add()
        {
            InitializeDropDownLists();

            return View();
        }

        // GET: Admin Assign
        public ActionResult Assign(int ID)
        {
            InitializeDropDownLists();
            if (!TryGetAppointmentDetails(ID, out var item))
                return RedirectToAction("Index", "Admin");

            SetAppointmentsViewBag(item);
            return View(item);
        }

        // POST: Admin Edit
        [HttpPost]
        public ActionResult Edit(int ID, Accounts account)
        {
            try
            {
                InitializeDropDownLists();

                if (!ModelState.IsValid)
                {
                    RefreshAccountDetails(ID);
                    return View();
                }

                bool isUpdateSuccessful = UpdateAccountDetails(ID, account);
                bool isSpecialUpdateSuccessful = UpdateSpecialDetails(ID, account);

                if (!isUpdateSuccessful || !isSpecialUpdateSuccessful)
                {
                    ViewBag.Message = "Failed to Update Record.";
                    return View();
                }

                ViewBag.Message = "Record Successfully Updated.";
                RefreshAccountDetails(ID);
                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // POST: Admin Add
        [HttpPost]
        public ActionResult Add(Accounts account)
        {
            try
            {
                InitializeDropDownLists();

                if (!ModelState.IsValid)
                {
                    return View(account);
                }

                bool isAddedSuccessfully = AddAccount(account);
                if (!isAddedSuccessfully)
                {
                    ViewBag.Message = "Failed to Add Record.";
                    return View(account);
                }

                ViewBag.Message = "Record Successfully Added.";
                return RedirectToAction("Index", "Admin");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // POST: Admin Assign
        [HttpPost]
        public ActionResult Assign(int ID, Appointments appointment)
        {
            try
            {
                InitializeDropDownLists();

                if (!ModelState.IsValid)
                {
                    RefreshAppointmentDetails(ID);
                    return View(appointment);
                }

                bool isAssignSuccessful = AssignDoctor(ID, appointment);

                if (!isAssignSuccessful)
                {
                    ViewBag.Message = "Failed to Assign Doctor.";
                    return View(appointment);
                }

                ViewBag.Message = "Doctor Successfully Assigned.";

                RefreshAppointmentDetails(ID);
                return RedirectToAction("Index", "Admin");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // POST: Admin Delete
        [HttpPost]
        public ActionResult Delete(int ID)
        {
            try
            {
                XmlUtil.DeleteAccount(ID);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool IsAdmin()
        {
            if (Session["Login"] == null)
            {
                return false;
            }

            Accounts obj = (Accounts)Session["Login"];
            return obj.AccountType.Equals(AccountType.Admin);
        }

        private void InitializeDropDownLists()
        {
            var accounts = LoadAccounts();

            Dictionary<string, string> GenderList = new Dictionary<string, string>()
            {
                {"Male", "Male"},
                {"Female", "Female"}
            };

            Dictionary<string, string> BloodList = new Dictionary<string, string>()
            {
                {"A+", "A+"},
                {"A-", "A-"},
                {"B+", "B+"},
                {"B-", "B-"},
                {"O+", "O+"},
                {"O-", "O-"},
                {"AB+", "AB+"},
                {"AB-", "AB-"}
            };

            Dictionary<string, string> AccountTypeList = new Dictionary<string, string>()
            {
                {"Doctor", "Doctor"},
                {"Patient", "Patient"}
            };

            List<Accounts> doctors = GetAccountType(accounts, AccountType.Doctor);
            Dictionary<int, string> DoctorList = new Dictionary<int, string>();

            foreach (var doctor in doctors)
            {
                DoctorList.Add(doctor.AccountID, $"{doctor.FirstName} {doctor.LastName}");
            }

            ViewBag.genderList = GenderList;
            ViewBag.bloodList = BloodList;
            ViewBag.accountTypeList = AccountTypeList;
            ViewBag.DoctorList = DoctorList;
        }

        private int GenerateUniqueID(List<Accounts> accounts, AccountType accountType)
        {
            int lastID = accounts
                .Where(a => a.AccountType == accountType)
                .Max(a => a.AccountID);

            int newID = lastID + 1;

            return newID;
        }

        private Accounts GetDoctorById(int doctorId)
        {
            var accounts = LoadAccounts();
            List<Accounts> doctors = GetAccountType(accounts, AccountType.Doctor);
            return doctors.FirstOrDefault(d => d.AccountID == doctorId);
        }

        private List<Accounts> GetAccountType(List<Accounts> accounts, AccountType accountType)
        {
            return accounts.Where(item => item.AccountType.Equals(accountType)).ToList();
        }

        private List<Appointments> GetAppointments(List<Appointments> appointments)
        {
            return appointments.Where(item => item.AppointmentDateTime > DateTime.Now).ToList();
        }

        private Accounts GetAccount(int ID)
        {
            var accounts = LoadAccounts();
            return accounts.FirstOrDefault(item => ID.Equals(item.AccountID));
        }

        private Appointments GetAppointment(int ID)
        {
            var appointments = LoadAppointments();
            return appointments.FirstOrDefault(item => ID.Equals(item.AppointmentID));
        }

        public bool AddAccount(Accounts account)
        {
            try
            {
                var accounts = LoadAccounts();

                int newID = GenerateUniqueID(accounts, account.AccountType);
                account.AccountID = newID;

                accounts.Add(account);

                XmlUtil.SaveAccountsToXml(accounts);

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error adding account: {ex.Message}");
                return false;
            }
        }

        private bool AssignDoctor(int ID, Appointments appointment)
        {
            var allAppointments = LoadAppointments();
            var existingAppointment = allAppointments.FirstOrDefault(item => ID.Equals(item.AppointmentID));

            if (existingAppointment == null) return false;

            // Look up the doctor using the ID
            var doctor = GetDoctorById(appointment.Doctor.DoctorID);

            if (doctor == null) return false;

            existingAppointment.Doctor.DoctorID = doctor.AccountID;
            existingAppointment.Doctor.FirstName = doctor.FirstName;
            existingAppointment.Doctor.LastName = doctor.LastName;

            XmlUtil.SaveAppointmentsToXml(allAppointments);
            return true;
        }

        private bool UpdateAccountDetails(int ID, Accounts account)
        {
            var allAccounts = LoadAccounts();
            var existingAccount = allAccounts.FirstOrDefault(item => ID.Equals(item.AccountID));

            if (existingAccount == null) return false;

            existingAccount.Password = account.Password;
            existingAccount.FirstName = account.FirstName;
            existingAccount.LastName = account.LastName;
            existingAccount.Gender = account.Gender;
            existingAccount.Dob = account.Dob;
            existingAccount.Email = account.Email;
            existingAccount.PhoneNo = account.PhoneNo;
            existingAccount.Address = account.Address;

            XmlUtil.SaveAccountsToXml(allAccounts);
            return true;
        }

        private bool UpdateSpecialDetails(int ID, Accounts account)
        {
            var allAccounts = LoadAccounts();
            var existingAccount = allAccounts.FirstOrDefault(item => ID.Equals(item.AccountID));

            if (existingAccount == null) return false;

            if (existingAccount.AccountType == AccountType.Doctor)
            {
                if (existingAccount.Doctor != null)
                {
                    existingAccount.Doctor.Specialization = account.Doctor.Specialization;
                    XmlUtil.SaveAccountsToXml(allAccounts);
                    return true;
                }
            }
            else if (existingAccount.AccountType == AccountType.Patient)
            {
                if (existingAccount.Patient != null)
                {
                    existingAccount.Patient.BloodGroup = account.Patient.BloodGroup;
                    XmlUtil.SaveAccountsToXml(allAccounts);
                    return true;
                }
            }
            return false;
        }

        private bool TryGetAccountDetails(int ID, out Accounts item)
        {
            item = GetAccount(ID);
            return item != null;
        }

        private bool TryGetAppointmentDetails(int ID, out Appointments item)
        {
            item = GetAppointment(ID);
            return item != null;
        }

        private void SetAccountsViewBag(Accounts item)
        {
            ViewBag.item = item;
            ViewBag.IsDoctor = item.AccountType.Equals(AccountType.Doctor);
            ViewBag.IsPatient = item.AccountType.Equals(AccountType.Patient);

            if (ViewBag.IsDoctor && item.Doctor != null)
            {
                ViewBag.Specialization = item.Doctor.Specialization;
            }

            if (ViewBag.IsPatient && item.Patient != null)
            {
                ViewBag.BloodGroup = item.Patient.BloodGroup;
            }
        }

        private void SetAppointmentsViewBag(Appointments item)
        {
            ViewBag.item = item;
        }

        private void RefreshAccountDetails(int ID)
        {
            if (TryGetAccountDetails(ID, out var item))
            {
                SetAccountsViewBag(item);
            }
            else
            {
                RedirectToAction("Index", "Admin");
            }
        }

        private void RefreshAppointmentDetails(int ID)
        {
            if (TryGetAppointmentDetails(ID, out var item))
            {
                SetAppointmentsViewBag(item);
            }
            else
            {
                RedirectToAction("Index", "Admin");
            }
        }

        private List<Accounts> LoadAccounts()
        {
            return XmlUtil.LoadAccountsXml();
        }

        private List<Appointments> LoadAppointments()
        {
            return XmlUtil.LoadAppointmentsXml();
        }

    }
}