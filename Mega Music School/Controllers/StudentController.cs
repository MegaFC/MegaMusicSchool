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
using Mega_Music_School.Enum;
using Microsoft.AspNetCore.Http;

namespace Mega_Music_School.Controllers
{
    public class StudentController : Controller
    {

        private readonly ApplicationDbContext _db;
        private readonly UserManager<UserAndAdminProfile> _userManager;
        private readonly SignInManager<UserAndAdminProfile> _signInManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IAccountService _accountService;

        public object VideoUpdate { get; private set; }

        // private IAccountService injectedAccountService;
        // private UserAndAdminProfile newInstanceOfUserAndAdminProfileModelAboutToBeCreated;

        public StudentController(ApplicationDbContext db, UserManager<UserAndAdminProfile> injectedUserManager, SignInManager<UserAndAdminProfile> injectedSignInManager,
            IWebHostEnvironment injectedWebHostEnvironment, IAccountService injectedAccountService)
        {
            _db = db;
            _userManager = injectedUserManager;
            _signInManager = injectedSignInManager;
            _webHostEnvironment = injectedWebHostEnvironment;
            _accountService = injectedAccountService;

        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
       
        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpGet]
        public IActionResult UpdateStudentProfile()
        {
            return View();
        }
        
        
        [HttpPost]
        // POST || PROFILEUPDATE
        public IActionResult UpdateStudentProfile(UserAndAdminProfile UserAndAdminProfileDetailsForReg)
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
                    ProfileDisplay = ProfilePicture(UserAndAdminProfileDetailsForReg.ProfilePicturePngJpg, "/StudentsPix/");
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

        //VIEW PROFILE
        [HttpGet]
        public IActionResult ViewProfile()
        {
            var userName = User.Identity.Name;
            if (userName != null)
            {
                var userDetails = _userManager.Users.Where(u => u.UserName == userName).Include(u => u.Country).Include(u => u.LocalGovernmentArea).Include(u => u.State).FirstOrDefault();
                return View(userDetails);
            }
            return View("ViewProfile", "Student");
        }

        //UPLOAD PROJECT
        [HttpGet]
        public IActionResult UploadVideo()
        {            
            {
                return View();
            }
        }

        [HttpPost]
        // POST || UPLOADPROJECT
        public IActionResult UploadVideo(Video videoUpdate)
        {
            string mp3FilePath = string.Empty;
            try
            {
                if (videoUpdate.VideoName == null)
                {
                    videoUpdate.Message = "Input Video Name To Continue.";
                    videoUpdate.ErrorHappened = true;
                    return View(videoUpdate);
                }

                if (videoUpdate.VideoLink == null)
                {
                    videoUpdate.Message = "Input Video Link To Continue.";
                    videoUpdate.ErrorHappened = true;
                    return View(videoUpdate);
                }

                if (videoUpdate.VideoDetails == null)
                {
                    videoUpdate.Message = "Write some details about the clip 2 continue.";
                    videoUpdate.ErrorHappened = true;
                    return View(videoUpdate);
                }

                if (videoUpdate.Mp3RawFile == null)
                {
                    videoUpdate.Message = "Input An MP3 Files.";
                    videoUpdate.ErrorHappened = true;
                    return View(videoUpdate);
                }

                // Query the user details if it exists in thr Db B4 Authentication

                var userDetails = _db.UserAndAdminProfiles.Where(s => s.UserName == User.Identity.Name)?.FirstOrDefault();
                if (userDetails != null)
                {

                    if (videoUpdate.Mp3RawFile != null)
                    {
                        var mp3Files = videoUpdate.Mp3RawFile.ContentType;
                        if (mp3Files.ToLower() == "audio/mpeg")
                        {
                            mp3FilePath = MpegUploadedFileContent(videoUpdate);
                        }
                        else
                        {
                            videoUpdate.Message = "File Only Accepts Mp3 Document Only";
                            videoUpdate.ErrorHappened = true;
                            return View(videoUpdate);
                        }

                    }

                    var newVideo = new Video
                    {
                        VideoName = videoUpdate.VideoName,
                        VideoDetails = videoUpdate.VideoDetails,
                        SongTitle = videoUpdate.SongTitle,
                        SongDetails = videoUpdate.SongDetails,
                        VideoLink = videoUpdate.VideoLink,
                        Mp3Upload = mp3FilePath,
                        StudentID = userDetails.Id,
                        VideoStatus = VideoStatus.Pending,
                        DateAdded = DateTime.Now,
                        Deleted = false,
                    };
                    _db.Add(newVideo);
                    _db.SaveChanges();
                    ModelState.Clear();


                    videoUpdate.Message = "Video Uploded Sucessfully";
                    videoUpdate.ErrorHappened = false;
                    return View();
                }
                else
                {
                    videoUpdate.Message = "Error Just Occur!!!";
                    videoUpdate.ErrorHappened = true;
                    return View();
                }
                
            }
            catch (Exception)
            {
                throw;
            }
        }


        //HERE WE ARE ONLY TRYING TO WRITE A METHOD ON THE FILE THAT WILL ONLY CONVERT THE UPLOADED ITEM TO MPEG ONLY
        public string MpegUploadedFileContent(Video filesSender)
        {
            string uniqueFileName = string.Empty;

            if (filesSender.Mp3RawFile != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "doctorUploads");
                string pathString = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "doctorUploads");
                if (!Directory.Exists(pathString))
                {
                    Directory.CreateDirectory(pathString);
                }
                uniqueFileName = Guid.NewGuid().ToString() + "_" + filesSender.Mp3RawFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    filesSender.Mp3RawFile.CopyTo(fileStream);
                }
            }
            var generatedPictureFilePath = "/doctorUploads/" + uniqueFileName;
            return generatedPictureFilePath;
        }


        //ALL STUDENT UPLOADS
        [HttpGet]
        public IActionResult AllUploadedItems()
        {
            {
                var allUploads = _accountService.StudentActionOnTheirUploads();
                return View(allUploads);
            }
        }

        [HttpPost]
        public IActionResult DeleteUpload(Guid studentId)
        {

            if (studentId != Guid.Empty)
            {
                //database quering... any database table should be passed with . your database name with the table
                var StudentChoiceOfAction = _db.Videos.Where(d => d.Id == studentId).FirstOrDefault();
                if (StudentChoiceOfAction != null)
                {
                    StudentChoiceOfAction.Deleted = true;

                    _db.Update(StudentChoiceOfAction);
                    _db.SaveChanges();
                    return Json(new { isError = false, msg = "Successful" });
                }
            }
            return Json(new { isError = true, msg = "Error" });
        }


        //STUDENT ADDING COURSE
        [HttpGet]
        public IActionResult CourseList()
        {
            var AddCourses = _accountService.StudentsAddCourses();
            return View(AddCourses);
        }


        [HttpPost]
        public IActionResult SelectCourse(Guid CourseDirectingId)
        {
            var user = User.Identity.Name;
            var userDetails = _userManager.Users.Where(u => u.UserName == user).FirstOrDefault();

            if (CourseDirectingId != Guid.Empty)
            {
                //database quering... HERE WE PASSED THE CONTENT OF THE COURSE TABLE TO STUDENT COURSE TABLE
                var CourseAdded = _db.Courses.Where(d => d.Id == CourseDirectingId).FirstOrDefault();
                if (CourseAdded != null && user != null)
                {
                    var courseToAdd = new StudentCourse
                    {
                        Deleted = false,
                        DateCourseAdded = DateTime.Now,
                        CourseId = CourseAdded.Id,
                        UserId = userDetails.Id,
                    };

                    _db.StudentCourse.Add(courseToAdd);
                    _db.SaveChanges();
                    return Json(new { isError = false, msg = "Successful" });
                }
            }
            return Json(new { isError = true, msg = "Error" });
        }


        [HttpGet]
        public IActionResult StudentAddedCourses()
        {
            var myAddedCourse = _accountService.PopulateMyCoursesList();
            return View(myAddedCourse);
        }



        //[HttpPost]
        //public JsonResult ChangeUsersPassword(string userProfileDetails, string id)
        //{
        //    if (userProfileDetails != null && id != null)
        //    {
        //        var currentUser = _userManager.Users.(id);
        //        if (currentUser != null)
        //        {
        //            var userDetails = JsonConvert.DeserializeObject<UserAndAdminProfile>(userProfileDetails);
        //            if (userDetails != null)
        //            {
        //                if (currentUser.Password != userDetails.Password)
        //                {
        //                    return Json(new { isError = true, msg = "Please enter the old password correctly" });
        //                }
        //                if (userDetails.NewPassword != userDetails.ConfirmPassword)
        //                {
        //                    return Json(new { isError = true, msg = "Password and confirm password must match" });
        //                }
        //                var result = _userManager.ChangePasswordAsync(currentUser, userDetails.Password, userDetails.NewPassword).Result;
        //                if (result.Succeeded)
        //                {
        //                    return Json(new { isError = false, msg = "User Password change Successfully", userId = currentUser.Id });
        //                }
        //                return Json(new { isError = false, msg = "Unable to change User Password", userId = currentUser.Id });
        //            }
        //            return Json(new { isError = true, msg = "Error occured" });
        //        }
        //    }
        //    return Json(new { isError = true, msg = "Error occured" });
        //}


        //public ApplicationUser GetUserByUserId(string id)
        //{
        //    if (id != null)
        //    {
        //        var user = _context.ApplicationUsers.Where(x => x.Id == id && !x.Deactivated).FirstOrDefault();
        //        if (user != null)
        //        {
        //            return user;
        //        }
        //    }
        //    return null;
        //}

    }
}