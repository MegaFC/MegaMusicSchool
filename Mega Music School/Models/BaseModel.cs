using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Mega_Music_School.Models
{
    public class BaseModel
    {
        [Key]
        public int? Id { get; set; }

        public string Name { get; set; }

        [NotMapped]
        public string Message { get; set; }

        [NotMapped]
        public bool ErrorHappened { get; set; }

    }
}
