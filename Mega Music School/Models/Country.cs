using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Mega_Music_School.Models
{
    public class Country : BaseModel
    {
        public bool Active { get; set; }
    }
}