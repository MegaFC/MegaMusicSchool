using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Mega_Music_School.Models
{
    public class State : BaseModel
    {
        public bool Active { get; set; }

        public int NationalityId { get; set; }
        [Display(Name = "Nationality")]
        [ForeignKey("NationalityId")]
        public virtual Country Nationality { get; set; }
    }
}
