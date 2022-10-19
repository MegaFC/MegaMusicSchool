using Mega_Music_School.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Mega_Music_School.ViewModel
{
    public class GeneralViewModel
    {
        [Key]
        public int Id { get; set; }
        public string Name{ get; set; }

        public int CountryId { get; set; }
        [Display(Name = "Country")]
        [ForeignKey("CountryId")]
        public virtual Country Country { get; set; }
    }
}
