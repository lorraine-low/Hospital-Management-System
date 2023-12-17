using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hospital_Management_System.Models
{
    public class Appointments
    {
        public int AppointmentID { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime AppointmentDateTime { get; set; }

        public string PatientRemarks { get; set; }

        public string Diagnosis { get; set; }

        public string ClinicRemarks { get; set; }

        public string Therapy { get; set; }

        public PatientViewModel Patient { get; set; }

        public DoctorViewModel Doctor { get; set; }

        public static List<Appointments> GetAllAppointments()
        {
            List<Appointments> appointments = new List<Appointments>
            {
                new Appointments {
                    AppointmentID = 70000001,
                    Patient = new PatientViewModel
                    {
                        PatientID = 30000001,
                        FirstName = "Emily",
                        LastName = "Johnson",
                    },
                    Doctor = new DoctorViewModel
                    {
                        DoctorID = 20000001,
                        FirstName = "William",
                        LastName = "Parker",
                    },
                    AppointmentDateTime = new DateTime(2023, 7, 27, 8, 15, 0),
                    PatientRemarks= "Booking an appointment for sore throat",
                    Diagnosis= "Streptococcal pharyngitis",
                    ClinicRemarks = "The patient is experiencing a case of bacterial strep throat, with reported pain starting on July 19th. They also have white spots and inflammation present on their tonsils.",
                    Therapy = "A course of amoxicillin for 10 days, once a day after a meal."
                }
            };
            return appointments;
        }
    }
}