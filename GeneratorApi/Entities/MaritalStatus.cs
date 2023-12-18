using GeneratorApi.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace GeneratorApi.Entities
{
    [Display(Name = "وضعیت تاهل")]
    public class MaritalStatus : BaseEntity
    {
        [Display(Name = "عنوان")]
        public string? Caption { get; set; }

        [Display(Name = "توضیحات")]
        public string? Description { get; set; }


    }
}
