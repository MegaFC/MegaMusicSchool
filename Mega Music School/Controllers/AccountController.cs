using Mega_Music_School.Database;
using Mega_Music_School.IHelper;
using Mega_Music_School.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mega_Music_School.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<UserAndAdminProfile> _userManager;
        private readonly SignInManager<UserAndAdminProfile> _signInManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IAccountService _accountService;
        public AccountController(ApplicationDbContext db, UserManager<UserAndAdminProfile> injectedUserManager, SignInManager<UserAndAdminProfile> injectedSignInManager,
            IWebHostEnvironment injectedWebHostEnvironment, IAccountService injectedAccountService)
        {
            _db = db;
            _userManager = injectedUserManager;
            _signInManager = injectedSignInManager;
            _webHostEnvironment = injectedWebHostEnvironment;
            _accountService = injectedAccountService;

        }

        //GET || Register
        [HttpGet]
        public IActionResult Register()
        {
            ViewBag.allDepartment = _accountService.GetAllTheDepartment();
            ViewBag.allCountry = _accountService.GetAllTheCountry();
            return View();
        }

        //GET || Register
        [HttpGet]
        public IActionResult StudentRegister()
        {
            ViewBag.allDepartment = _accountService.GetAllTheDepartment();
            ViewBag.allCountry = _accountService.GetAllTheCountry();
            return View();
        }


        [HttpPost]
       // POST || Register
        public async Task<IActionResult> Register(UserAndAdminProfile UserAndAdminProfileDetailsForReg)
        {
            try
            {


                // We are Validating User Details For Register
                // ViewBag.MedexDepartment = ourAccountService.GetAllTheDepartment();
                ViewBag.allDepartment = _accountService.GetAllTheDepartment();
                ViewBag.allCountry = _accountService.GetAllTheCountry();


                if (UserAndAdminProfileDetailsForReg.Email == null)
                {
                    UserAndAdminProfileDetailsForReg.Message = "Your Email was empty,please put mail.";
                    UserAndAdminProfileDetailsForReg.ErrorHappened = true;
                    return View(UserAndAdminProfileDetailsForReg);
                }

                if (UserAndAdminProfileDetailsForReg.FirstName == null || UserAndAdminProfileDetailsForReg.LastName == null)
                {
                    UserAndAdminProfileDetailsForReg.Message = " Please put in your first and last name.";
                    UserAndAdminProfileDetailsForReg.ErrorHappened = true;
                    return View(UserAndAdminProfileDetailsForReg);
                }

                if (UserAndAdminProfileDetailsForReg.PhoneNumber == null)
                {
                    UserAndAdminProfileDetailsForReg.Message = "Please put in your phone number.";
                    UserAndAdminProfileDetailsForReg.ErrorHappened = true;
                    return View(UserAndAdminProfileDetailsForReg);
                }

                if (UserAndAdminProfileDetailsForReg.Password != UserAndAdminProfileDetailsForReg.ConfirmPassword)
                {
                    UserAndAdminProfileDetailsForReg.Message = "Mismatched Password Enter Correct Password.";
                    UserAndAdminProfileDetailsForReg.ErrorHappened = true;
                    return View(UserAndAdminProfileDetailsForReg);
                }

                if (UserAndAdminProfileDetailsForReg.DepartmentId == 0)
                {
                    UserAndAdminProfileDetailsForReg.Message = "Please enter your Department.";
                    UserAndAdminProfileDetailsForReg.ErrorHappened = true;
                    return View(UserAndAdminProfileDetailsForReg);
                }

                // Query the user details if it exists in thr Db B4 Authentication
                var queryUserAndAdminProfileTableWithEmail =  _userManager.Users.Where(s => s.Email == UserAndAdminProfileDetailsForReg.Email)?.FirstOrDefault();
                if (queryUserAndAdminProfileTableWithEmail != null)
                {
                    UserAndAdminProfileDetailsForReg.Message = "Your email aready exist, Enter new email";
                    UserAndAdminProfileDetailsForReg.ErrorHappened = true;
                    return View(UserAndAdminProfileDetailsForReg);
                }

                var queryUserAndAdminProfileDetailWithPhoneNumber =  _userManager.Users.Where(s => s.PhoneNumber == UserAndAdminProfileDetailsForReg.PhoneNumber)?.FirstOrDefault();
                if (queryUserAndAdminProfileDetailWithPhoneNumber != null)
                {
                    UserAndAdminProfileDetailsForReg.Message = "Your phone number aready exist in our database,  Enter new phone number";
                    UserAndAdminProfileDetailsForReg.ErrorHappened = true;
                    return View(UserAndAdminProfileDetailsForReg);
                }

                var newInstanceOfUserAndAdminProfileModelAboutToBCreated = new UserAndAdminProfile();
                {
                    newInstanceOfUserAndAdminProfileModelAboutToBCreated.FirstName = UserAndAdminProfileDetailsForReg.FirstName;
                    newInstanceOfUserAndAdminProfileModelAboutToBCreated.LastName = UserAndAdminProfileDetailsForReg.LastName;
                    newInstanceOfUserAndAdminProfileModelAboutToBCreated.Email = UserAndAdminProfileDetailsForReg.Email;
                    newInstanceOfUserAndAdminProfileModelAboutToBCreated.PhoneNumber = UserAndAdminProfileDetailsForReg.PhoneNumber;
                    newInstanceOfUserAndAdminProfileModelAboutToBCreated.Password = UserAndAdminProfileDetailsForReg.Password;
                    newInstanceOfUserAndAdminProfileModelAboutToBCreated.UserName = UserAndAdminProfileDetailsForReg.UserName;
                    newInstanceOfUserAndAdminProfileModelAboutToBCreated.CountryId = UserAndAdminProfileDetailsForReg.CountryId;
                    newInstanceOfUserAndAdminProfileModelAboutToBCreated.StateId = UserAndAdminProfileDetailsForReg.StateId;
                    newInstanceOfUserAndAdminProfileModelAboutToBCreated.LGAId = UserAndAdminProfileDetailsForReg.LGAId;
                    newInstanceOfUserAndAdminProfileModelAboutToBCreated.JoiningDate = DateTime.Now;
                    newInstanceOfUserAndAdminProfileModelAboutToBCreated.Address = UserAndAdminProfileDetailsForReg.Address;
                    newInstanceOfUserAndAdminProfileModelAboutToBCreated.AcademicQualification = UserAndAdminProfileDetailsForReg.AcademicQualification;
                    newInstanceOfUserAndAdminProfileModelAboutToBCreated.DateOfBirth = UserAndAdminProfileDetailsForReg.DateOfBirth;
                };


                var cretedUserandAdmin=  await _userManager.CreateAsync(newInstanceOfUserAndAdminProfileModelAboutToBCreated, UserAndAdminProfileDetailsForReg.Password);
                if (cretedUserandAdmin.Succeeded)
                {
                    //To create a student portion
                   var addToRole = await _userManager.AddToRoleAsync(newInstanceOfUserAndAdminProfileModelAboutToBCreated, "Admin");
                    await _signInManager.SignInAsync(newInstanceOfUserAndAdminProfileModelAboutToBCreated, isPersistent: true);


                    UserAndAdminProfileDetailsForReg.Message = "User Created Succesfully.";
                    UserAndAdminProfileDetailsForReg.ErrorHappened = false;
                    return RedirectToAction("Index", "Admin"); // change for admin & user
                }
                else
                {
                    UserAndAdminProfileDetailsForReg.Message = "Internal Error Occured";
                    UserAndAdminProfileDetailsForReg.ErrorHappened = true;
                    return View(UserAndAdminProfileDetailsForReg);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        // POST || Register
        public async Task<IActionResult> StudentRegister(UserAndAdminProfile UserAndAdminProfileDetailsForReg)
        {
            try
            {
                // We are Validating User Details For Register
                ViewBag.allDepartment = _accountService.GetAllTheDepartment();
                ViewBag.allCountry = _accountService.GetAllTheCountry();


                if (UserAndAdminProfileDetailsForReg.Email == null)
                {
                    UserAndAdminProfileDetailsForReg.Message = "Your Email was empty,please put mail.";
                    UserAndAdminProfileDetailsForReg.ErrorHappened = true;
                    return View(UserAndAdminProfileDetailsForReg);
                }

                if (UserAndAdminProfileDetailsForReg.FirstName == null || UserAndAdminProfileDetailsForReg.LastName == null)
                {
                    UserAndAdminProfileDetailsForReg.Message = " Please put in your first and last name.";
                    UserAndAdminProfileDetailsForReg.ErrorHappened = true;
                    return View(UserAndAdminProfileDetailsForReg);
                }

                if (UserAndAdminProfileDetailsForReg.PhoneNumber == null)
                {
                    UserAndAdminProfileDetailsForReg.Message = "Please put in your phone number.";
                    UserAndAdminProfileDetailsForReg.ErrorHappened = true;
                    return View(UserAndAdminProfileDetailsForReg);
                }


                if (UserAndAdminProfileDetailsForReg.Password != UserAndAdminProfileDetailsForReg.ConfirmPassword)
                {
                    UserAndAdminProfileDetailsForReg.Message = "Mismatched Password Enter Correct Password.";
                    UserAndAdminProfileDetailsForReg.ErrorHappened = true;
                    return View(UserAndAdminProfileDetailsForReg);
                }


                if (UserAndAdminProfileDetailsForReg.DepartmentId == 0)
                {
                    UserAndAdminProfileDetailsForReg.Message = "Please enter your Department.";
                    UserAndAdminProfileDetailsForReg.ErrorHappened = true;
                    return View(UserAndAdminProfileDetailsForReg);
                }

                // Query the user details if it exists in thr Db B4 Authentication
                var queryUserAndAdminProfileTableWithEmail = _userManager.Users.Where(s => s.Email == UserAndAdminProfileDetailsForReg.Email)?.FirstOrDefault();
                if (queryUserAndAdminProfileTableWithEmail != null)
                {
                    UserAndAdminProfileDetailsForReg.Message = "Your email aready exist, Enter new email";
                    UserAndAdminProfileDetailsForReg.ErrorHappened = true;
                    return View(UserAndAdminProfileDetailsForReg);
                }

                var queryUserAndAdminProfileDetailWithPhoneNumber = _userManager.Users.Where(s => s.PhoneNumber == UserAndAdminProfileDetailsForReg.PhoneNumber)?.FirstOrDefault();
                if (queryUserAndAdminProfileDetailWithPhoneNumber != null)
                {
                    UserAndAdminProfileDetailsForReg.Message = "Your phone number aready exist inour database,  Enter new phone number";
                    UserAndAdminProfileDetailsForReg.ErrorHappened = true;
                    return View(UserAndAdminProfileDetailsForReg);
                }

                var newInstanceOfUserAndAdminProfileModelAboutToBCreated = new UserAndAdminProfile();
                {
                    newInstanceOfUserAndAdminProfileModelAboutToBCreated.FirstName = UserAndAdminProfileDetailsForReg.FirstName;
                    newInstanceOfUserAndAdminProfileModelAboutToBCreated.LastName = UserAndAdminProfileDetailsForReg.LastName;
                    newInstanceOfUserAndAdminProfileModelAboutToBCreated.Email = UserAndAdminProfileDetailsForReg.Email;
                    newInstanceOfUserAndAdminProfileModelAboutToBCreated.PhoneNumber = UserAndAdminProfileDetailsForReg.PhoneNumber;
                    newInstanceOfUserAndAdminProfileModelAboutToBCreated.DepartmentId = UserAndAdminProfileDetailsForReg.DepartmentId;
                    newInstanceOfUserAndAdminProfileModelAboutToBCreated.UserName = UserAndAdminProfileDetailsForReg.UserName;
                    newInstanceOfUserAndAdminProfileModelAboutToBCreated.CountryId = UserAndAdminProfileDetailsForReg.CountryId;
                    newInstanceOfUserAndAdminProfileModelAboutToBCreated.StateId = UserAndAdminProfileDetailsForReg.StateId;
                    newInstanceOfUserAndAdminProfileModelAboutToBCreated.LGAId = UserAndAdminProfileDetailsForReg.LGAId;
                    newInstanceOfUserAndAdminProfileModelAboutToBCreated.JoiningDate = DateTime.Now;
                    newInstanceOfUserAndAdminProfileModelAboutToBCreated.Address = UserAndAdminProfileDetailsForReg.Address;
                    newInstanceOfUserAndAdminProfileModelAboutToBCreated.AcademicQualification = UserAndAdminProfileDetailsForReg.AcademicQualification;
                    newInstanceOfUserAndAdminProfileModelAboutToBCreated.DateOfBirth = UserAndAdminProfileDetailsForReg.DateOfBirth;
                };


                var cretedUserandAdmin = await _userManager.CreateAsync(newInstanceOfUserAndAdminProfileModelAboutToBCreated, UserAndAdminProfileDetailsForReg.Password);
                if (cretedUserandAdmin.Succeeded)
                {
                    //To create a student portion
                    var addToRole = await _userManager.AddToRoleAsync(newInstanceOfUserAndAdminProfileModelAboutToBCreated, "Student");
                    await _signInManager.SignInAsync(newInstanceOfUserAndAdminProfileModelAboutToBCreated, isPersistent: true);

                    UserAndAdminProfileDetailsForReg.Message = "User Created Succesfully.";
                    UserAndAdminProfileDetailsForReg.ErrorHappened = false;
                    return RedirectToAction("Index", "Student");
                }
                else
                {
                    UserAndAdminProfileDetailsForReg.Message = "Internal Error Occured";
                    UserAndAdminProfileDetailsForReg.ErrorHappened = true;
                    return View(UserAndAdminProfileDetailsForReg);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }



        //GET || Log-in
        [HttpGet]
        public IActionResult LogIn(string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        //POST || Log-in
        [HttpPost]
        public async Task<IActionResult> LogIn(UserAndAdminProfile UserAndAdminProfileDetailsForLogin)
        {
            try
            {

                // We are Validating User Details For Login
                if (UserAndAdminProfileDetailsForLogin.Email == null)
                {
                   UserAndAdminProfileDetailsForLogin.Message = "Your mail was empty,please put mail.";
                    UserAndAdminProfileDetailsForLogin.ErrorHappened = true;
                    return View(UserAndAdminProfileDetailsForLogin);

                }
                if (UserAndAdminProfileDetailsForLogin.Password == null)
                {
                    UserAndAdminProfileDetailsForLogin.Message = "Your password was empty,please put in your password before logging in.";
                    UserAndAdminProfileDetailsForLogin.ErrorHappened = true;
                    return View(UserAndAdminProfileDetailsForLogin);
                }
                //  Validating User Details For Login stops here

                // Query  4  the user with email detail if it exists in thr Db B4 Authentication
                var queryUserAndAdminProfileTableWithEmail = _userManager.FindByEmailAsync(UserAndAdminProfileDetailsForLogin.Email).Result;
                if (queryUserAndAdminProfileTableWithEmail == null)
                {
                    UserAndAdminProfileDetailsForLogin.Message = "Your details was not found in our database, please register.";
                    UserAndAdminProfileDetailsForLogin.ErrorHappened = true;
                    return View(UserAndAdminProfileDetailsForLogin);
                }

                if (queryUserAndAdminProfileTableWithEmail.Expelled == true)
                {
                    UserAndAdminProfileDetailsForLogin.Message = "Senior Man You Are Already Expelled!";
                    UserAndAdminProfileDetailsForLogin.ErrorHappened = true;
                    return View(UserAndAdminProfileDetailsForLogin);
                }

                // End of Query  4  the user details if it exists in thr Db B4 Authentication


                // We are using Ef Core IDDB SignIn Method with the params it needed to sign the user In & if it Succeeded it goes to the Dashboard
                var result = await _signInManager.PasswordSignInAsync(queryUserAndAdminProfileTableWithEmail, UserAndAdminProfileDetailsForLogin.Password, true, false);
                if (result.Succeeded)
                {
                    //To create a definite portion
                    var userRole =  _userManager.GetRolesAsync(queryUserAndAdminProfileTableWithEmail).Result.FirstOrDefault();
                    if(userRole == "Admin")
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Student");
                    }
                }
                // We are using Ef Core IDDB SignIn Method with the params it needed to sign the user In & if it Succeeded it goes to the Dashboard
                UserAndAdminProfileDetailsForLogin.Message = "wrong password, log-in failed.";
                UserAndAdminProfileDetailsForLogin.ErrorHappened = true;
                return View(UserAndAdminProfileDetailsForLogin);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public JsonResult GetStateInCountry(int countryId)
        {
            var getStates = _accountService.GetAllTheStateAttachedToACountry(countryId);
            return Json(new SelectList(getStates, "Id", "Name"));
        }

        [HttpGet]
        public JsonResult GetLGAInState(int StateId)
        {
            var getlgaGovt = _accountService.GetAllTheLGAsAttachedToAState(StateId);
            return Json(new SelectList(getlgaGovt, "Id", "Name"));

        }



    }
}
