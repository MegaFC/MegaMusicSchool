using Mega_Music_School.ViewModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Mega_Music_School.Models
{
    public class StudentCourse
    {
        [Key]
        public Guid Id { get; set; }
        
        [DisplayName("Course Deleted")]
        public bool Deleted { get; set; }

        [DisplayName("Date Added")]
        public DateTime DateCourseAdded { get; set; }

        //public string Course { get; set; }
        //[DisplayName("Course Name")]
        //[ForeignKey("CourseId")]
        //public string CourseName { get; set; }

        public Guid CourseId { get; set; }
        [Display(Name = "Course")]
        [ForeignKey("CourseId")]
        public virtual Course Course { get; set; }

        public string UserId { get; set; }
        [Display(Name = "User")]
        [ForeignKey("UserId")]
        public virtual UserAndAdminProfile UserAndAdminProfile { get; set; }

    }
}
