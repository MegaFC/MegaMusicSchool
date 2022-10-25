using Mega_Music_School.Database;
using Mega_Music_School.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mega_Music_School.IHelper;
using Mega_Music_School.ViewModel;
using Mega_Music_School.Enum;
using Microsoft.AspNetCore.Http;

namespace Mega_Music_School.Controllers
{
    public class AdminController : Controller
    {

        private readonly ApplicationDbContext _db;
        private readonly UserManager<UserAndAdminProfile> _userManager;
        private readonly SignInManager<UserAndAdminProfile> _signInManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IAccountService _accountService;


        public AdminController(ApplicationDbContext db, UserManager<UserAndAdminProfile> injectedUserManager, SignInManager<UserAndAdminProfile> injectedSignInManager,
            IWebHostEnvironment injectedWebHostEnvironment, IAccountService injectedAccountService)
        {
            _db = db;
            _userManager = injectedUserManager;
            _signInManager = injectedSignInManager;
            _webHostEnvironment = injectedWebHostEnvironment;
            _accountService = injectedAccountService;
        }

        
        public IActionResult Index()
        {
            return View();
        }


        //ADD / ADD DEPARTMENT
        [HttpGet]
        public IActionResult AddDepartment()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddDepartment(Department departmentData)
        {

            if (departmentData.Name == null)
            {
                departmentData.Message = "Add your department name to continue";
                departmentData.ErrorHappened = true;
                return View(departmentData);
            }
            else
            {
                var departmentCheck = _db.Departments.Where(d => d.Name == departmentData.Name).FirstOrDefault();
                if (departmentCheck == null)
                {
                    var newDepartment = new Department()
                    {
                        Name = departmentData.Name,
                        Active = true,
                    };
                    _db.Departments.Add(newDepartment);
                    _db.SaveChanges();
                    ModelState.Clear();

                    departmentData.Message = "New department saved succesfully";
                    departmentData.ErrorHappened = false;
                    return View();

                }
            }
            departmentData.Message = "Internal error occured retry.";
            departmentData.ErrorHappened = true;
            return View(departmentData);
        }

        //UPDATE PROFILE
        [HttpGet]
        public IActionResult UpdateAdminProfile()
        {
            return View();
        }


        [HttpPost]
        // POST || PROFILEUPDATE
        public IActionResult UpdateAdminProfile(UserAndAdminProfile UserAndAdminProfileDetailsForReg)
        {
            try
            {
                var ProfileDisplay = string.Empty;

                if (UserAndAdminProfileDetailsForReg.ProfilePicturePngJpg == null)
                {
                    UserAndAdminProfileDetailsForReg.Message = "Your Picture.";
                    UserAndAdminProfileDetailsForReg.ErrorHappened = true;
                    return View(UserAndAdminProfileDetailsForReg);
                }
                else
                {
                    ProfileDisplay = ProfilePicture(UserAndAdminProfileDetailsForReg.ProfilePicturePngJpg, "/AdminsPix/");
                }

                //ViewBag.allDepartment = _accountService.GetAllTheDepartment();
                //ViewBag.allCountry = _accountService.GetAllTheCountry();


                if (UserAndAdminProfileDetailsForReg.Address == null)
                {
                    UserAndAdminProfileDetailsForReg.Message = "Your Email was empty,please put mail.";
                    UserAndAdminProfileDetailsForReg.ErrorHappened = true;
                    return View(UserAndAdminProfileDetailsForReg);
                }

                if (UserAndAdminProfileDetailsForReg.DateOfBirth == null)
                {
                    UserAndAdminProfileDetailsForReg.Message = " Please put in your first and last name.";
                    UserAndAdminProfileDetailsForReg.ErrorHappened = true;
                    return View(UserAndAdminProfileDetailsForReg);
                }

                if (UserAndAdminProfileDetailsForReg.JoiningDate == null)
                {
                    UserAndAdminProfileDetailsForReg.Message = "Please put in your phone number.";
                    UserAndAdminProfileDetailsForReg.ErrorHappened = true;
                    return View(UserAndAdminProfileDetailsForReg);
                }

                if (UserAndAdminProfileDetailsForReg.AcademicQualification == null)
                {
                    UserAndAdminProfileDetailsForReg.Message = "Please enter your password.";
                    UserAndAdminProfileDetailsForReg.ErrorHappened = true;
                    return View(UserAndAdminProfileDetailsForReg);
                }
                if (UserAndAdminProfileDetailsForReg.ProfilePicturePngJpg == null)
                {
                    UserAndAdminProfileDetailsForReg.Message = "Please enter your password.";
                    UserAndAdminProfileDetailsForReg.ErrorHappened = true;
                    return View(UserAndAdminProfileDetailsForReg);
                }

                // Query the user details if it exists in thr Db B4 Authentication
                var existingUserDetails = _userManager.Users.Where(s => s.UserName == User.Identity.Name)?.FirstOrDefault();
                if (existingUserDetails != null)

                    existingUserDetails.Address = UserAndAdminProfileDetailsForReg.Address;
                existingUserDetails.DateOfBirth = UserAndAdminProfileDetailsForReg.DateOfBirth;
                existingUserDetails.JoiningDate = UserAndAdminProfileDetailsForReg.JoiningDate;
                existingUserDetails.AcademicQualification = UserAndAdminProfileDetailsForReg.AcademicQualification;
                existingUserDetails.ProfilePictureUrl = ProfileDisplay;

                _db.UserAndAdminProfiles.Update(existingUserDetails);
                _db.SaveChanges();
                ModelState.Clear();


                existingUserDetails.Message = "You have Successfully Updated Your Profile, Go To View";
                existingUserDetails.ErrorHappened = false;
                return View();
            }
            catch (Exception)
            {
                throw;
            }
        }

        //METHOD TO VIEW PERSON'S PROFILE PIC
        //To make profile pix showing
        //We collected user's profile pics and create a method which collects it, and store them in one directory 

        public string ProfilePicture(IFormFile filesUrl, string fileLocation)
        {
            string uniqueFileName = string.Empty;

            if (filesUrl != null)
            {
                var upPath = fileLocation.Trim('/');
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, upPath);

                string pathString = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", upPath);
                if (!Directory.Exists(pathString))
                {
                    Directory.CreateDirectory(pathString);
                }
                uniqueFileName = Guid.NewGuid().ToString() + "_" + filesUrl.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    filesUrl.CopyTo(fileStream);
                }
            }
            var generatedPictureFilePath = fileLocation + uniqueFileName;
            return generatedPictureFilePath;
        }

        [HttpGet]
        public IActionResult ViewProfile()
        {
            var userName = User.Identity.Name;
            if (userName != null)
            {
                var userDetails = _userManager.Users.Where(u => u.UserName == userName).Include(u => u.Country).Include(u => u.LocalGovernmentArea).Include(u => u.State).FirstOrDefault();
                return View(userDetails);
            }
            return RedirectToAction("login", "Accounts");
        }

        //DASHBOARD
        [HttpGet]
        public IActionResult Dashboard()
        {
            return View();
        }

        //DASHBOARD
        [HttpGet]
        public IActionResult UpdateDatabase()
        {
            return View();
        }


        //ADD / DROP COURSE

        [HttpGet]
        public IActionResult CreateCourse()
        {
            ViewBag.allDepartment = _accountService.GetAllTheDepartment();
            return View();
        }

        [HttpPost]
        public IActionResult CreateCourse(Course courseData)
        {
            string courseHandbookPath = string.Empty;
            string courseImagePath = string.Empty;

            ViewBag.allDepartment = _accountService.GetAllTheDepartment();

            if (courseData.CourseName == null)
            {
                courseData.Message = "Add your course name to continue";
                courseData.ErrorHappened = true;
                return View(courseData);
            }
            if (courseData.CourseTitle == null)
            {
                courseData.Message = "Add your course title to continue";
                courseData.ErrorHappened = true;
                return View(courseData);
            }
            if (courseData.CourseHandbook== null)
            {
                courseData.Message = "Add your course handbook to continue";
                courseData.ErrorHappened = true;
                return View(courseData);
            }

            if (courseData.Image == null)
            {
                courseData.Message = "Add a course image to continue";
                courseData.ErrorHappened = true;
                return View(courseData);
            }
           
            if (courseData.DepartmentId == 0)
            {
                courseData.Message = "Select your course Department";
                courseData.ErrorHappened = true;
                return View(courseData);
            }

            var courseCheck = _db.Courses.Where(d => d.CourseName == courseData.CourseName).FirstOrDefault();
            if (courseCheck == null)
            {
                if (courseData.CourseHandbook != null)
                {
                    var handbookfiletype = courseData.CourseHandbook.ContentType;
                    if (handbookfiletype.ToLower() == "application/pdf")
                    {
                        courseHandbookPath = UploadedHandbook(courseData);
                    }
                    else
                    {
                        courseData.Message = "Handbook accepts only pdf";
                        courseData.ErrorHappened = true;
                        return View(courseData);
                    }
                }

                if (courseData.Image!= null)
                {
                    var handbookfiletype = courseData.Image.ContentType;
                    if (handbookfiletype.ToLower() == "image/jpeg" || handbookfiletype.ToLower() == "image/png")
                    {
                        courseImagePath = UploadedFileForImage(courseData);
                    }
                    else
                    {
                        courseData.Message = "Image accepts only png/jpg";
                        courseData.ErrorHappened = true;
                        return View(courseData);
                    }

                }

                var newCourse = new Course()
                {
                    CourseName = courseData.CourseName,
                    CourseTitle = courseData.CourseTitle,
                    CourseHandbookURL = courseHandbookPath,
                    CourseImageURL = courseImagePath,
                    Deleted = false,
                    DepartmentId = courseData.DepartmentId,
                };
                _db.Courses.Add(newCourse);
                _db.SaveChanges();
                ModelState.Clear();


                courseData.Message = " New course saved succesfully";
                courseData.ErrorHappened = false;
                return View();
            }
            else
            {
                courseData.Message = "A Course with this name Already exist.";
                courseData.ErrorHappened = true;
                return View(courseData);
            }
        }


        //We collected user's profile pics and create a method which collects it, and store them in one directory 
        public string UploadedHandbook(Course filesSender)
        {
            string uniqueFileName = string.Empty;

            if (filesSender.CourseHandbook != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "doctorUploads");
                string pathString = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "doctorUploads");
                if (!Directory.Exists(pathString))
                {
                    Directory.CreateDirectory(pathString);
                }
                uniqueFileName = Guid.NewGuid().ToString() + "_" + filesSender.CourseHandbook.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    filesSender.CourseHandbook.CopyTo(fileStream);
                }
            }
            var generatedPictureFilePath = "/doctorUploads/" + uniqueFileName;
            return generatedPictureFilePath;
        }

        public string UploadedFileForImage(Course filesSender)
        {
            string uniqueFileName = string.Empty;

            if (filesSender.Image != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads");
                string pathString = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads");
                if (!Directory.Exists(pathString))
                {
                    Directory.CreateDirectory(pathString);
                }
                uniqueFileName = Guid.NewGuid().ToString() + "_" + filesSender.Image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    filesSender.Image.CopyTo(fileStream);
                }
            }
            var generatedPictureFilePath = "/Uploads/" + uniqueFileName;
            return generatedPictureFilePath;
        }

        //To make profile pix showing
        //We collected user's profile pics and create a method which collects it, and store them in one directory 
        public string ProfilePicture(UserAndAdminProfile filesSender)
        {
            string uniqueFileName = string.Empty;

            if (filesSender.ProfilePicturePngJpg != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "doctorUploads");
                string pathString = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "doctorUploads");
                if (!Directory.Exists(pathString))
                {
                    Directory.CreateDirectory(pathString);
                }

                //uniqueFileName = string().ToString() "_" + filesSender.ProfilePicturePngJpg.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    filesSender.ProfilePicturePngJpg.CopyTo(fileStream);
                }
            }
            var generatedPictureFilePath = "/doctorUploads/" + uniqueFileName;
            return generatedPictureFilePath;
        }


        //ADD / ADD NATIONALITY
        [HttpGet]
        public IActionResult NationalityCreate()
        {
            return View();
        }

        [HttpPost]
        public IActionResult NationalityCreate(Country countryData)
        {

            if (countryData.Name == null)
            {
                countryData.Message = "Add a Country Name to continue";
                countryData.ErrorHappened = true;
                return View(countryData);
            }
            else
            {
                var countryCheck = _db.Departments.Where(d => d.Name == countryData.Name).FirstOrDefault();
                if (countryCheck == null)
                {
                    var newCountry = new Country()
                    {
                        Name = countryData.Name,
                        Active = true,
                    };
                    _db.Countries.Add(newCountry);
                    _db.SaveChanges();
                    ModelState.Clear();


                    countryData.Message = " New Country Created succesfully";
                    countryData.ErrorHappened = false;
                    return View();
                }
            }
            countryData.Message = "Internal error occured retry.";
            countryData.ErrorHappened = true;
            return View(countryData);
        }

        //STATE CREATE
        public IActionResult StateCreate()
        {
            ViewBag.allCountry = _accountService.GetAllTheCountry();
            return View();
        }

        [HttpPost]
        public IActionResult StateCreate(State stateData)
        {
            ViewBag.allCountry = _accountService.GetAllTheCountry();
            if (stateData.Name == null)
            {
                stateData.Message = "Input a State Name";
                stateData.ErrorHappened = true;
                return View(stateData);
            }
            if (stateData.NationalityId == 0)
            {
                stateData.Message = "Add a Country Name to continue";
                stateData.ErrorHappened = true;
                return View(stateData);
            }
            else
            {
                var stateCheck = _db.Departments.Where(d => d.Name == stateData.Name).FirstOrDefault();
                if (stateCheck == null)
                {
                    var newState = new State()
                    {
                        Name = stateData.Name,
                        NationalityId = stateData.NationalityId,
                        Active = true,
                    };
                    _db.State.Add(newState);
                    _db.SaveChanges();
                    ModelState.Clear();


                    stateData.Message = " New state created succesfully";
                    stateData.ErrorHappened = false;
                    return View();
                }
            }
            stateData.Message = "Internal error occured retry.";
            stateData.ErrorHappened = true;
            return View(stateData);
        }


        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }



        //LGA CREATE
        public IActionResult LgaCreate()
        {
            ViewBag.allCountry = _accountService.GetAllTheCountry();
            return View();
        }
        [HttpPost]
        public IActionResult LgaCreate(LGA lgaData)
        {
            ViewBag.allCountry = _accountService.GetAllTheCountry();
            if (lgaData.Name == null)
            {
                lgaData.Message = "Add your Lga to continue";
                lgaData.ErrorHappened = true;
                return View(lgaData);
            }
            if (lgaData.StateId == 0)
            {
                lgaData.Message = "Add your State to continue";
                lgaData.ErrorHappened = true;
                return View(lgaData);
            }

            if (lgaData.CountryId == 0)
            {
                lgaData.Message = "Add The Country this state belongs to";
                lgaData.ErrorHappened = true;
                return View(lgaData);
            }
            else
            {
                var lgaCheck = _db.Departments.Where(d => d.Name == lgaData.Name).FirstOrDefault();
                if (lgaCheck == null)
                {
                    var newLga = new LGA()
                    {
                        Name = lgaData.Name,
                        StateId = lgaData.StateId,
                        Active = true,
                    };
                    _db.LGAs.Add(newLga);
                    _db.SaveChanges();
                    ModelState.Clear();


                    lgaData.Message = " New Local Government created succesfully";
                    lgaData.ErrorHappened = false;
                    return View();
                }
            }
            lgaData.Message = "Internal error occured retry.";
            lgaData.ErrorHappened = true;
            return View(lgaData);
        }



        //View Country
        [HttpGet]
        public IActionResult AllCountry()
        {
            var allCountries = _accountService.PopulateListOfCountriesToTable();
            return View(allCountries);
        }

        //View allDepartments
        [HttpGet]
        public IActionResult AllDepartment()
        {
            var allDepartments = _accountService.PopulateListOfDepartmentsToTable();
            return View(allDepartments);
        }

        //View ALL LGA
        [HttpGet]
        public IActionResult AllLGA()
        {
            var allLGA = _accountService.PopulateListOfLGAToTable();
            return View(allLGA);
        }

        //View ALL STATE
        [HttpGet]
        public IActionResult AllState()
        {
            return View();
        }

        //Populate All Video
        [HttpGet]
        public IActionResult VideoStatus()
        {
            var allVideos = _accountService.PopulateListOfVideoToTable();
            return View(allVideos);
        }


        //Get All Approved Video
        [HttpGet]
        public IActionResult Entertainment()
        {
            var allVideos = _accountService.ApprovedVideo();
            return View(allVideos);
        }

        [HttpPost]
        public JsonResult AcceptVideos(Guid videoId)
        {

            if (videoId != Guid.Empty)
            {
                //database quering... any database table should be passed with . your database name with the table
                var videoToApproveDetails = _db.Videos.Where(d => d.Id == videoId).FirstOrDefault();
                if (videoToApproveDetails != null)
                {
                    videoToApproveDetails.VideoStatus = Enum.VideoStatus.Accepted;

                    _db.Videos.Update(videoToApproveDetails);
                    _db.SaveChanges();
                    return Json(new { isError = false, msg = "Successful" });
                }
            }
            return Json(new { isError = true, msg = "Not Found" });

        }


        [HttpPost]
        public IActionResult RejectVideos(Guid videoId)
        {

            if (videoId != Guid.Empty)
            {
                //database quering... any database table should be passed with . your database name with the table
                var videoToRejectDetails = _db.Videos.Where(d => d.Id == videoId).FirstOrDefault();
                if (videoToRejectDetails != null)
                {
                    videoToRejectDetails.VideoStatus = Enum.VideoStatus.Rejected;

                    _db.Videos.Update(videoToRejectDetails);
                    _db.SaveChanges();
                    ModelState.Clear();
                    return Json(new { isError = false, msg = "Successful" });
                }
            }
            return Json(new { isError = true, msg = "Not Found" });
        }


        [HttpGet]
        public IActionResult AllAddedCourses()
        {
            {
                var allCourses = _accountService.PopulateListOfCoursesAddedForTheAdminToTakeAction();
                return View(allCourses);
            }
        }

        [HttpPost]
        public IActionResult DeleteCourse(Guid courseId)
        {

            if (courseId != Guid.Empty)
            {
                //database quering... any database table should be passed with . your database name with the table
                var coursesToBeDeleted = _db.Courses.Where(d => d.Id == courseId).FirstOrDefault();
                if (coursesToBeDeleted != null)
                {
                    coursesToBeDeleted.Deleted = true;

                    _db.Update(coursesToBeDeleted);
                    _db.SaveChanges();
                    return Json(new { isError = false, msg = "Successful" });
                }
            }
            return Json(new { isError = true, msg = "Error" });
        }


        //UPDATE PROFILE
        [HttpGet]
        public async Task<IActionResult>AdminEffect()
        {
            var allStudent = _accountService.PopulateListOfStudentInTheScheme().Result;
            return View(allStudent);
        }

        [HttpPost]
        public IActionResult DeleteStudent(string studentField)
        {

            if (studentField != string.Empty)
            {
                //database quering... any database table should be passed with . your database name with the table
                var studentToBeDeleted = _db.UserAndAdminProfiles.Where(d => d.Id == studentField).FirstOrDefault();
                if (studentToBeDeleted != null)
                {
                    studentToBeDeleted.Expelled = true;

                    _db.Update(studentToBeDeleted);
                    _db.SaveChanges();
                    return Json(new { isError = false, msg = "Successful" });
                }
            }
            return Json(new { isError = true, msg = "Error" });
        }


    }
}