using GeneratorApi.Entities.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeneratorApi.Entities
{
    [Display(Name = "کاربران")]
    public class User : IdentityUser<int>, IEntity<int>
    {
        [Display(Name = "نام")]
        public string FName { get; set; }

        [Display(Name = "نام خانوادگی")]
        public string LName { get; set; }

        [Display(Name = "شماره ملی")]
        public string NationalNo { get; set; }

        [Display(Name = "شماره پرسنلی")]
        public string? PCode { get; set; }

        [Display(Name = "شماره همراه")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "آدرس پست الکترونیک")]
        public string? Email { get; set; }

        [Display(Name = "تاریخ انقضاء")]
        public DateTime? UserExpireDate { get; set; }


    }

    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(p => p.UserName).IsRequired().HasMaxLength(100);
        }
    }
}
