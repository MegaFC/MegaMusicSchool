using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Mega_Music_School.Enum
{
    public enum VideoStatus
    {
        [Description("Pending")]
        Accepted = 1,

        [Description("Rejected")]
        Rejected = 2,

        [Description("Accepted")]
        Pending = 3,
    }
}
