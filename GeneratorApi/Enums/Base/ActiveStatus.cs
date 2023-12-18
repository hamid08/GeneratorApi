using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneratorApi.Enums.Base
{
    public enum ActiveStatus
    {
        [Description("فعال")]
        [Display(Name = "فعال")]
        Active = 0,

        [Description("غیر فعال")]
        [Display(Name = "غیر فعال")]
        NotActive = 1
    }
}
