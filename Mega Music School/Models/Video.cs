using Mega_Music_School.Enum;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;


namespace Mega_Music_School.Models
{
    public class Video
    {
        [Key]
        public Guid Id { get; set; }

        [DisplayName("Video Name")]
        public string VideoName { get; set; }

        [DisplayName("Video Link")]
        public string VideoLink { get; set; }

        [DisplayName("Video Details")]
        public string VideoDetails { get; set; }

        [DisplayName("Song Title")]
        public string SongTitle { get; set; }

        [DisplayName("Song Details")]
        public string SongDetails { get; set; }

        [DisplayName("Mp3 Upload URL File Path")]
        public string Mp3Upload { get; set; }
        [NotMapped]
        public IFormFile Mp3RawFile { get; set; }

        [DisplayName("Date Added")]
        public DateTime DateAdded { get; set; }

        [NotMapped]
        public string Message { get; set; }
        [NotMapped]
        public bool ErrorHappened { get; set; }

        [DisplayName("Deleted")]
        public bool Deleted { get; set; }

        [DisplayName ("Video Status")]
        public VideoStatus VideoStatus { get; set; }


        public string StudentID { get; set; }
        [Display(Name = "Student adding the video")]
        [ForeignKey("StudentID")]
        public virtual UserAndAdminProfile Student { get; set; }

    }
}
