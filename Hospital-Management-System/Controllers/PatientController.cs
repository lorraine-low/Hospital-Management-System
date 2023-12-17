/*
* Class: PatientController
* Author: Lorraine Low
* Date: 30/07/2023
* FileName: PatientController.cs
* Purpose: This class serves as a Controller for patient actions. 
* It controls the interactions between the Patient Model and the corresponding views. 
*/

using Hospital_Management_System.Models;
using Hospital_Management_System.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace Hospital_Management_System.Controllers
{
    public class PatientController : Controller
    {
        // GET: Patient
        //This method displays the Patient view Index page if a valid patient session is detected. 
        //It redirects to the Login index page otherwise.
        public ActionResult Index()
        {
            if (!IsPatient())
            {
                return Redirect("../Login/index");
            }

            Accounts currentAccount = (Accounts)Session["Login"];
            int AccountID = currentAccount.AccountID;

            var appointments = LoadAppointments();
            ViewBag.AppointmentList = GetAppointments(appointments, AccountID);

            return View();
        }

        // GET: Patient Add
        //This method displays the Add Appointment view with a new appointment instance.
        public ActionResult Add()
        {
            return View(new Appointments());
        }

        [HttpPost]
        //This method attempts to add a new appointment with the provided details.
        //It validates and processes the form post, adding the appointment if possible.
        //It redirects to the Patient index after a successful addition.
        public ActionResult Add(Appointments appointment, string AppointmentDate, string AppointmentTime)
        {
            var appointments = LoadAppointments();
            var formPostResult = HandleAddFormPost(appointment, AppointmentDate, AppointmentTime, appointments);

            if (!formPostResult.success)
            {
                ViewBag.Message = formPostResult.errorMessage;
                return View(appointment);
            }

            ViewBag.Message = "Appointment Successfully Scheduled.";
            return RedirectToAction("Index", "Patient");
        }

        private List<Appointments> GetAppointments(List<Appointments> appointments, int ID)
        {
            return appointments.Where(item => item.Patient.PatientID == ID).ToList();
        }

        private bool IsPatient()
        {
            if (Session["Login"] == null)
            {
                return false;
            }

            Accounts obj = (Accounts)Session["Login"];
            return obj.AccountType.Equals(AccountType.Patient);
        }

        private bool AddAppointment(Appointments appointment, List<Appointments> appointments)
        {
            try
            {
                int newID = GenerateUniqueID(appointments);
                appointment.AppointmentID = newID;

                AddDoctor(appointment);

                appointments.Add(appointment);

                XmlUtil.SaveAppointmentsToXml(appointments);

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error adding account: {ex.Message}");
                return false;
            }
        }

        private int GenerateUniqueID(List<Appointments> appointments)
        {
            int lastID = appointments
                .Max(a => a.AppointmentID);

            int newID = lastID + 1;

            return newID;
        }

        private void AddAppointmentDateTime(Appointments appointment, string appointmentDate, string appointmentTime)
        {
            DateTime date = DateTime.ParseExact(appointmentDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            DateTime time = DateTime.ParseExact(appointmentTime, "HH:mm", CultureInfo.InvariantCulture);

            DateTime dateTime = new DateTime(
                date.Year, date.Month, date.Day,
                time.Hour, time.Minute, time.Second, time.Millisecond);

            appointment.AppointmentDateTime = dateTime;
        }

        private void AddDoctor(Appointments appointment)
        {
            if (appointment.Doctor == null)
            {
                appointment.Doctor = new DoctorViewModel
                {
                    DoctorID = -1,
                    FirstName = "",
                    LastName = "",
                };
            }
        }

        private bool AddPatient(Appointments appointment)
        {
            if (!(Session["Login"] is Accounts account))
            {
                return false;
            }

            appointment.Patient = new PatientViewModel
            {
                PatientID = account.AccountID,
                FirstName = account.FirstName,
                LastName = account.LastName,
            };

            return true;
        }

        private bool AppointmentExists(DateTime dateTime, int patientId, List<Appointments> appointments)
        {
            return appointments.Any(a => a.AppointmentDateTime == dateTime && a.Patient.PatientID == patientId);
        }

        private (bool success, string errorMessage) HandleAddFormPost(Appointments appointment, string appointmentDate, string appointmentTime, List<Appointments> appointments)
        {
            if (!AddPatient(appointment))
            {
                return (false, "You are not logged in.");
            }

            AddAppointmentDateTime(appointment, appointmentDate, appointmentTime);

            if (!ModelState.IsValid)
            {
                return (false, "The model state is not valid.");
            }

            if (AppointmentExists(appointment.AppointmentDateTime, appointment.Patient.PatientID, appointments))
            {
                return (false, "You already have an appointment scheduled for this date and time.");
            }

            bool isAddedSuccessfully = AddAppointment(appointment, appointments);
            if (!isAddedSuccessfully)
            {
                return (false, "Failed to Schedule Appointment.");
            }

            return (true, null);
        }

        private List<Appointments> LoadAppointments()
        {
            return XmlUtil.LoadAppointmentsXml();
        }

    }
}
