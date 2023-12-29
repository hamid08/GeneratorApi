using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GeneratorApi.Extensions.Grid
{
    public static class EnumExtensions
    {
        public static string ToDescription(this Enum enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<DescriptionAttribute>()
                            .Description;
        }

        public static bool IsNullableEnum(this Type type)
        {
            Type u = Nullable.GetUnderlyingType(type);
            return u != null && u.IsEnum;
        }
    }
}
