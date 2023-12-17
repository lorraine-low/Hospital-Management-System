using System.Collections.Generic;
using System;
using System.Linq;
using System.Xml.Linq;
using Hospital_Management_System.Models;

namespace Hospital_Management_System.Services
{
    public class XmlUtil
    {
        //Used in actual web browser runtime
        private static readonly string accountsFilePath = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Accounts.xml");

        //Used in actual web browser runtime
        private static readonly string appointmentsFilePath = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Appointments.xml");

        public static void CreateAccountsXml()
        {
            var accounts = Accounts.GetAllAccounts();

            XDocument accountsDoc = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement("Accounts",
                    from account in accounts
                    select new XElement("Account",
                        new XElement("AccountID", account.AccountID),
                        new XElement("Password", account.Password),
                        new XElement("AccountType", account.AccountType.ToString()),
                        new XElement("FirstName", account.FirstName),
                        new XElement("LastName", account.LastName),
                        new XElement("Gender", account.Gender),
                        new XElement("Dob", account.Dob),
                        new XElement("Email", account.Email),
                        new XElement("PhoneNo", account.PhoneNo),
                        new XElement("Address", account.Address),
                        account.AccountType == AccountType.Admin ?
                        new XElement("Admin",
                            new XElement("Role", account.Admin.Role)
                        ) : null,
                        account.AccountType == AccountType.Doctor ?
                        new XElement("Doctor",
                            new XElement("Specialization", account.Doctor.Specialization)
                        ) : null,
                        account.AccountType == AccountType.Patient ?
                        new XElement("Patient",
                            new XElement("BloodGroup", account.Patient.BloodGroup)
                        ) : null
                    )
                )
            );

            //accountsDoc.Save();
            accountsDoc.Save(accountsFilePath);
        }

        public static List<Accounts> LoadAccountsXml()
        {
            XDocument accountsXml = XDocument.Load(accountsFilePath);

            var accounts = from account in accountsXml.Descendants("Account")
                           select new Accounts
                           {
                               AccountID = (int)account.Element("AccountID"),
                               Password = (string)account.Element("Password"),
                               AccountType = (AccountType)Enum.Parse(typeof(AccountType), (string)account.Element("AccountType")),
                               FirstName = (string)account.Element("FirstName"),
                               LastName = (string)account.Element("LastName"),
                               Gender = (string)account.Element("Gender"),
                               Dob = DateTime.Parse((string)account.Element("Dob")),
                               Email = (string)account.Element("Email"),
                               PhoneNo = (string)account.Element("PhoneNo"),
                               Address = (string)account.Element("Address"),
                               Admin = account.Element("Admin") != null ? new Admins
                               {
                                   Role = (string)account.Element("Admin").Element("Role"),
                               } : null,
                               Doctor = account.Element("Doctor") != null ? new Doctors
                               {
                                   Specialization = (string)account.Element("Doctor").Element("Specialization")
                               } : null,
                               Patient = account.Element("Patient") != null ? new Patients
                               {
                                   BloodGroup = (string)account.Element("Patient").Element("BloodGroup"),
                               } : null
                           };

            return accounts.ToList();
        }

        public static void SaveAccountsToXml(List<Accounts> accounts)
        {
            XDocument accountsDoc = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement("Accounts",
                    from account in accounts
                    select new XElement("Account",
                        new XElement("AccountID", account.AccountID),
                        new XElement("Password", account.Password),
                        new XElement("AccountType", account.AccountType.ToString()),
                        new XElement("FirstName", account.FirstName),
                        new XElement("LastName", account.LastName),
                        new XElement("Gender", account.Gender),
                        new XElement("Dob", account.Dob),
                        new XElement("Email", account.Email),
                        new XElement("PhoneNo", account.PhoneNo),
                        new XElement("Address", account.Address),
                        account.AccountType == AccountType.Admin ?
                        new XElement("Admin",
                            new XElement("Role", account.Admin?.Role)
                        ) : null,
                        account.AccountType == AccountType.Doctor ?
                        new XElement("Doctor",
                            new XElement("Specialization", account.Doctor?.Specialization)
                        ) : null,
                        account.AccountType == AccountType.Patient ?
                        new XElement("Patient",
                            new XElement("BloodGroup", account.Patient?.BloodGroup)
                        ) : null
                    )
                )
            );

            accountsDoc.Save(accountsFilePath);
        }

        public static void DeleteAccount(int accountId)
        {
            var accounts = LoadAccountsXml();

            var account = accounts.FirstOrDefault(a => a.AccountID == accountId);

            if (account != null)
            {
                accounts.Remove(account);

                SaveAccountsToXml(accounts);
            }
        }

        public static void CreateAppointmentsXml()
        {
            var appointments = Appointments.GetAllAppointments();

            XDocument appointmentsDoc = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement("Appointments",
                    from appointment in appointments
                    select new XElement("Appointment",
                        new XElement("AppointmentID", appointment.AppointmentID),
                        new XElement("Patient",
                            new XElement("PatientID", appointment.Patient.PatientID),
                            new XElement("FirstName", appointment.Patient.FirstName),
                            new XElement("LastName", appointment.Patient.LastName)
                        ),
                        new XElement("Doctor",
                            new XElement("DoctorID", appointment.Doctor.DoctorID),
                            new XElement("FirstName", appointment.Doctor.FirstName),
                            new XElement("LastName", appointment.Doctor.LastName)
                        ),
                        new XElement("AppointmentDateTime", appointment.AppointmentDateTime),
                        new XElement("PatientRemarks", appointment.PatientRemarks),
                        new XElement("Diagnosis", appointment.Diagnosis),
                        new XElement("ClinicRemarks", appointment.ClinicRemarks),
                        new XElement("Therapy", appointment.Therapy)
                    )
                )
            );
            appointmentsDoc.Save(appointmentsFilePath);
        }

        public static List<Appointments> LoadAppointmentsXml()
        {
            XDocument appointmentsXml = XDocument.Load(appointmentsFilePath);

            var appointments = from appointment in appointmentsXml.Descendants("Appointment")
                               let dateTimeElement = appointment.Element("AppointmentDateTime")
                               let parsedDateTime = DateTime.TryParse(dateTimeElement != null ? dateTimeElement.Value : string.Empty, out DateTime tempVal) ? tempVal : default(DateTime)
                               let patientElement = appointment.Element("Patient")
                               let doctorElement = appointment.Element("Doctor")
                               select new Appointments
                               {
                                   AppointmentID = (int)appointment.Element("AppointmentID"),
                                   Patient = patientElement != null ? new PatientViewModel
                                   {
                                       PatientID = (int)patientElement.Element("PatientID"),
                                       FirstName = (string)patientElement.Element("FirstName"),
                                       LastName = (string)patientElement.Element("LastName"),
                                   } : null,
                                   Doctor = doctorElement != null ? new DoctorViewModel
                                   {
                                       DoctorID = (int)doctorElement.Element("DoctorID"),
                                       FirstName = (string)doctorElement.Element("FirstName"),
                                       LastName = (string)doctorElement.Element("LastName"),
                                   } : null,
                                   AppointmentDateTime = parsedDateTime,
                                   PatientRemarks = (string)appointment.Element("PatientRemarks"),
                                   Diagnosis = (string)appointment.Element("Diagnosis"),
                                   ClinicRemarks = (string)appointment.Element("ClinicRemarks"),
                                   Therapy = (string)appointment.Element("Therapy")
                               };

            return appointments.ToList();
        }

        public static void SaveAppointmentsToXml(List<Appointments> appointments)
        {
            XDocument appointmentsDoc = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement("Appointments",
                    from appointment in appointments
                    select new XElement("Appointment",
                        new XElement("AppointmentID", appointment.AppointmentID),
                        appointment.Patient != null ?
                        new XElement("Patient",
                            new XElement("PatientID", appointment.Patient.PatientID),
                            new XElement("FirstName", appointment.Patient.FirstName),
                            new XElement("LastName", appointment.Patient.LastName)
                        ) : null,
                        appointment.Doctor != null ?
                        new XElement("Doctor",
                            new XElement("DoctorID", appointment.Doctor.DoctorID),
                            new XElement("FirstName", appointment.Doctor.FirstName),
                            new XElement("LastName", appointment.Doctor.LastName)
                        ) : null,
                        new XElement("AppointmentDateTime", appointment.AppointmentDateTime),
                        new XElement("PatientRemarks", appointment.PatientRemarks),
                        new XElement("Diagnosis", appointment.Diagnosis),
                        new XElement("ClinicRemarks", appointment.ClinicRemarks),
                        new XElement("Therapy", appointment.Therapy)
                    )
                )
            );
            appointmentsDoc.Save(appointmentsFilePath);
        }
    }
}