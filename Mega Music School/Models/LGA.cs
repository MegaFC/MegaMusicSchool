using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Mega_Music_School.Models
{
    public class LGA : BaseModel
    {
        public bool Active { get; set; }

        public int StateId { get; set; }
        [Display(Name = "State")]
        [ForeignKey("StateId")]
        public virtual State State { get; set; }

        public int? CountryId { get; set; }
        [Display(Name = "Country")]
        [ForeignKey("CountryId")]
        public virtual Country Country { get; set; }

    }
}
