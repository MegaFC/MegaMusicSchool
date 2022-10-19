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
    public class Course : BaseModel
    {
        [Key]
        public new Guid Id { get; set; }

        [DisplayName("Course Name")]
        public string CourseName { get; set; }

        
        [DisplayName("Course Title")] 
        public string CourseTitle { get; set; }

        
        [DisplayName("Course Handbook URL")]
        public string CourseHandbookURL{ get; set; }


        [DisplayName("Course Handbook")]
        [NotMapped]
        public IFormFile CourseHandbook { get; set; }


        [DisplayName("Course Image")]
        public string CourseImageURL { get; set; }


        [DisplayName("Image")]
        [NotMapped]
        public IFormFile Image { get; set; }

        
        [DisplayName("Course Deleted")]
        public bool Deleted { get; set; }


        //Foreign Key for Department
        public int DepartmentId { get; set; }
        [Display(Name = "Department")]
        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }
    }
}
