using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Mega_Music_School.Models
{
    public class UserAndAdminProfile : IdentityUser
    {
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        
        [NotMapped]
        public string Password { get; set; }
        [NotMapped]
        public string ConfirmPassword { get; set; }
        
        [DisplayName ("Date Registered")]
        public DateTime JoiningDate { get; set; }
        
        [DisplayName("Profile Picture")]
        public string ProfilePictureUrl { get; set; }       
        [NotMapped]
        public IFormFile ProfilePicturePngJpg { get; set; }
        
        [DisplayName("Address")]
        public string Address { get; set; }

        [DisplayName("Highest Academic Qualification")]
        public string AcademicQualification { get; set; }

        [DisplayName("Date Of Birth")]
        public DateTime DateOfBirth { get; set; }

        [NotMapped]
        public string Message { get; set; }
        [NotMapped]
        public bool ErrorHappened { get; set; }

        public bool Expelled { get; set; }

        // All Foreign Keys Model References Starts Here
        public int? CountryId { get; set; }
        [Display(Name = "Country")]
        [ForeignKey("CountryId")]
        public virtual Country Country { get; set; }

        public int? StateId { get; set; }
        [Display(Name = "State")]
        [ForeignKey("StateId")]
        public virtual State State { get; set; }

        public int? LGAId { get; set; }
        [Display(Name = "Local Government Area")]
        [ForeignKey("LGAId")]
        public virtual LGA LocalGovernmentArea { get; set; }

        public int? DepartmentId { get; set; }
        [Display(Name = "Department Name")]
        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }
        //End Of Foreign Key Model
    }
}
