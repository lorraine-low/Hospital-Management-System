/*
* Class: DoctorController
* Author: Lorraine Low
* Date: 30/07/2023
* FileName: DoctorController.cs
* Purpose: This class serves as a Controller for doctor related actions. 
* It controls the interactions between the Doctor Model and the corresponding Views. 
* It assumes the use of an MVC (Model-View-Controller) architecture and a valid session state.
*/

using Hospital_Management_System.Models;
using Hospital_Management_System.Services;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Hospital_Management_System.Controllers
{
    // This Controller class is used for doctor-related operations
    public class DoctorController : Controller
    {
        /*
        * Method: Index
        * Purpose: It returns the Doctor view Index page if a valid doctor session is detected. 
        * It redirects to the Login index page otherwise.
        */
        public ActionResult Index()
        {
            if (!IsDoctor())
            {
                return Redirect("../Login/index");
            }

            Accounts currentAccount = (Accounts)Session["Login"];
            int AccountID = currentAccount.AccountID;

            var appointments = LoadAppointments();
            ViewBag.AppointmentList = GetAppointments(appointments, AccountID);

            return View();
        }

        /*
        * Method: Edit (GET)
        * Purpose: It returns the Edit view with appointment details if a valid appointment ID is found. 
        * It redirects to the Doctor index page otherwise.
        */
        public ActionResult Edit(int ID)
        {
            if (!TryGetAppointmentDetails(ID, out var item))
                return RedirectToAction("Index", "Doctor");

            SetAppointmentsViewBag(item);
            return View(item);
        }

        /*
        * Method: Edit (POST)
        * Purpose: It validates and updates the treatment details of an appointment. 
        * It redirects to the Doctor index page after successful update.
        */
        [HttpPost]
        public ActionResult Edit(int ID, Appointments appointment)
        {
            if (!ModelState.IsValid)
            {
                RefreshAppointmentDetails(ID);
                return View(appointment);
            }

            bool isUpdateSuccessful = UpdateTreatmentDetails(ID, appointment);

            if (!isUpdateSuccessful)
            {
                ViewBag.Message = "Failed to Update Treatment Details.";
                return View(appointment);
            }

            ViewBag.Message = "Treatment Details Successfully Updated.";

            RefreshAppointmentDetails(ID);
            return RedirectToAction("Index", "Doctor");
        }

        private bool IsDoctor()
        {
            if (Session["Login"] == null)
            {
                return false;
            }

            Accounts obj = (Accounts)Session["Login"];
            return obj.AccountType.Equals(AccountType.Doctor);
        }

        private Appointments GetAppointment(int ID)
        {
            var appointments = LoadAppointments();
            return appointments.FirstOrDefault(item => ID.Equals(item.AppointmentID));
        }

        private List<Appointments> GetAppointments(List<Appointments> appointments, int ID)
        {
            return appointments.Where(item => item.Doctor.DoctorID == ID).ToList();
        }

        private bool UpdateTreatmentDetails(int ID, Appointments appointment)
        {
            var allAppointments = LoadAppointments();
            var existingAppointment = allAppointments.FirstOrDefault(item => ID.Equals(item.AppointmentID));

            if (existingAppointment == null) return false;

            existingAppointment.Diagnosis = appointment.Diagnosis;
            existingAppointment.ClinicRemarks = appointment.ClinicRemarks;
            existingAppointment.Therapy = appointment.Therapy;

            XmlUtil.SaveAppointmentsToXml(allAppointments);
            return true;
        }

        private bool TryGetAppointmentDetails(int ID, out Appointments item)
        {
            item = GetAppointment(ID);
            return item != null;
        }

        private void SetAppointmentsViewBag(Appointments item)
        {
            ViewBag.item = item;
        }

        private void RefreshAppointmentDetails(int ID)
        {
            if (TryGetAppointmentDetails(ID, out var item))
            {
                SetAppointmentsViewBag(item);
            }
            else
            {
                RedirectToAction("Index", "Doctor");
            }
        }

        private List<Appointments> LoadAppointments()
        {
            return XmlUtil.LoadAppointmentsXml();
        }

    }
}