using GeneratorApi.Entities.Base;
using GeneratorApi.Filters;
using System.ComponentModel.DataAnnotations;

namespace GeneratorApi.Entities
{
    [Display(Name = "وضعیت تاهل")]
    public class MaritalStatus : BaseEntity
    {
        [Display(Name = "عنوان")]
        [Unique]
        public string? Caption { get; set; }

        [Display(Name = "توضیحات")]
        public string? Description { get; set; }

        [Unique]

        public int Port { get; set; }

        public ICollection<Brand> Brands { get; set; }
    }
}
