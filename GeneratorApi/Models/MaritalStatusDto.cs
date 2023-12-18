using GeneratorApi.Api;
using GeneratorApi.Entities;
using System.ComponentModel.DataAnnotations;

namespace GeneratorApi.Models
{
    public class MaritalStatusDto : BaseDto<MaritalStatusDto, MaritalStatus>
    {
        [Display(Name = "عنوان")]
        public string? Caption { get; set; }

        [Display(Name = "توضیحات")]
        public string? Description { get; set; }

       
    }

    public class MaritalStatusSelectDto : BaseDto<MaritalStatusSelectDto, MaritalStatus>
    {
        [Display(Name = "عنوان")]
        public string? Caption { get; set; }

        [Display(Name = "توضیحات")]
        public string? Description { get; set; }



    }
}
