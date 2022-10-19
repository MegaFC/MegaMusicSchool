using Mega_Music_School.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Mega_Music_School.ViewModel
{
    public class CourseViewModel
    {
        [Key]
        public Guid CourseId { get; set; }

        public string CourseName { get; set; }

        public string CourseTitle { get; set; }

        public string CourseHandbook { get; set; }

        public string HandbookURL { get; set; }

        public string CourseImage { get; set; }
        
        public IFormFile ImageURL { get; set; }

        public bool Deleted { get; set; }

        [DisplayName("Course Department")]

        public virtual Department Department { get; set; }

        public string UserId { get; set; }
        [Display(Name = "User")]
        [ForeignKey("UserId")]
        public virtual UserAndAdminProfile UserAndAdminProfile { get; set; }

    }
}
