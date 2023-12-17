/*
* Class: LoginController
* Author: Lorraine Low
* Date: 30/07/2023
* FileName: LoginController.cs
* Purpose: This class serves as a Controller for login actions. 
* It controls the interactions between the Accounts Model and the Login view. 
* It assumes the use of an MVC (Model-View-Controller) architecture and a valid session state.
*/

using Hospital_Management_System.Models;
using Hospital_Management_System.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Hospital_Management_System.Controllers
{
    public class LoginController : Controller
    {

        // GET: Login
        //This method displays the Login view when a GET request is received.
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        //This method attempts to log the user in with the provided login information.
        //It validates the login info, attempts to find a matching account,
        //and redirects the user based on their account type.
        public ActionResult Index(Accounts loginInfo)
        {
            var validationResult = ValidateLoginInfo(loginInfo);
            if (validationResult != null)
            {
                return validationResult;
            }

            var accounts = XmlUtil.LoadAccountsXml();
            var currentAccount = FindMatchingAccount(loginInfo, accounts);

            if (currentAccount == null)
            {
                ViewBag.Message = "Invalid Login.";
                return View(loginInfo);
            }

            Session["Login"] = currentAccount;

            var redirectUrl = GetRedirectUrl(currentAccount);
            return Redirect(redirectUrl);
        }

        private ActionResult ValidateLoginInfo(Accounts loginInfo)
        {
            if (loginInfo?.Email == null || loginInfo?.Password == null)
            {
                ViewBag.Message = "Invalid Login.";
                return View(loginInfo);
            }

            loginInfo.Email = loginInfo.Email.ToUpper().Trim();
            loginInfo.Password = loginInfo.Password.Trim();

            return null;
        }

        private Accounts FindMatchingAccount(Accounts loginInfo, List<Accounts> accounts)
        {
            return accounts.FirstOrDefault(
                account => account?.Email?.ToUpper() == loginInfo.Email &&
                           account?.Password?.Trim() == loginInfo.Password);
        }

        private string GetRedirectUrl(Accounts currentAccount)
        {
            switch (currentAccount.AccountType)
            {
                case AccountType.Admin:
                    return "Admin/index";
                case AccountType.Patient:
                    return "Patient/index";
                case AccountType.Doctor:
                    return "Doctor/index";
                default:
                    throw new InvalidOperationException($"Unknown account type: {currentAccount.AccountType}");
            }
        }

        //This method logs the user out by clearing the Login session and redirecting to the Home index.
        public ActionResult Logout()
        {
            Session["Login"] = null;
            return RedirectToAction("Index", "Home");
        }
    }
}