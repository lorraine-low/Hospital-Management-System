/*
* Class: HomeController
* Author: Lorraine Low
* Date: 30/07/2023
* FileName: HomeController.cs
* Purpose: This class serves as a Controller for general site navigation.
* It controls the interactions between various non-specific views. 
*/

using System.Web.Mvc;

namespace Hospital_Management_System.Controllers
{
    public class HomeController : Controller
    {
        // Base path for all the service views
        private const string ServiceViewPath = "~/Views/Home/Services/";

        // GET: Home
        //This method displays the Home view.
        public ActionResult Index()
        {
            return View();
        }

        //This method displays the About view.
        public ActionResult About()
        {
            return View();
        }

        //This method displays the Contact view.
        public ActionResult Contact()
        {
            return View();
        }

        //This method displays the Services Index view.
        public ActionResult ServicesIndex()
        {
            return View(ServiceViewPath + "Index.cshtml");
        }

        //This method displays the Emergency Care view.
        public ActionResult EmergencyCare()
        {
            return View(ServiceViewPath + "EmergencyCare.cshtml");
        }

        //This method displays the Specialist Consultation view.
        public ActionResult SpecialistConsultation()
        {
            return View(ServiceViewPath + "SpecialistConsultation.cshtml");
        }

        //This method displays the Maternity Care view.
        public ActionResult MaternityCare()
        {
            return View(ServiceViewPath + "MaternityCare.cshtml");
        }

        //This method displays the Health Screening view.
        public ActionResult HealthScreening()
        {
            return View(ServiceViewPath + "HealthScreening.cshtml");
        }
    }
}