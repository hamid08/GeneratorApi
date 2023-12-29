using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace GeneratorApi.Extensions.Grid
{
    public static class GridExtensions
    {
        public enum SortType : byte
        {
            Ascending = 1,
            Descending = 2
        }

        public enum FilterType // Sort Is Important (Numbers)
        {
            [Description("later_then_or_equal")]
            later_then_or_equal = 1,

            [Description("earlier_then_or_equal")]
            earlier_then_or_equal = 2,


            [Description("less_then_or_equal")]
            less_then_or_equal = 3,

            [Description("greater_then_or_equal")]
            greater_then_or_equal = 4,

            [Description("not_equals")]
            not_equals = 5,

            [Description("starts_with")]
            starts_with = 6,

            [Description("ends_with")]
            ends_with = 7,

            [Description("less_then")]
            less_then = 8,

            [Description("greater_then")]
            greater_then = 9,

            [Description("earlier_then")]
            earlier_then = 10,

            [Description("later_then")]
            later_then = 11,

            [Description("contains")]
            contains = 12,

            [Description("equals")]
            equals = 13,

        }

        public static async Task<GridDto<T>> ApplySearchFilters<T>(this IQueryable<T> data, BaseRequestGridDto request, CancellationToken cancellationToken) where T : class
        {
            try
            {

                var filters = Enum.GetValues(typeof(FilterType))
                    .Cast<FilterType>()
                    .ToList();

                var numberTypes = new Type[] { typeof(byte), typeof(byte?), typeof(int), typeof(int?), typeof(long), typeof(long?) };


                //Get All Params From QueryString (Correct Data)
                var parameters = request.Filters.Select(c => new { Key = ((string)c.Key).ToLower(), Value = c.Value.ToString().ToLower().Trim() })
                    .Where(c => !string.IsNullOrEmpty(c.Key) && c.Key != "_" && !string.IsNullOrEmpty(c.Value))
                    .Select(c =>
                    {
                        var filter = filters.FirstOrDefault(x => c.Key.EndsWith(x.ToDescription()));
                        var key = filter > 0 ? c.Key.Substring(0, c.Key.LastIndexOf($"-{filter.ToDescription()}")) : c.Key;
                        var finalKey = "";
                        var keySplit = key.Split('-');
                        for (int i = 0; i < keySplit.Length; i++)
                        {
                            finalKey += keySplit[i];
                        }

                        if (filter > 0)
                        {
                            finalKey += $"|{filter.ToDescription()}";
                        }

                        return new { Key = finalKey, c.Value };


                    }).ToList();

                //

                List<Expression> orExpressions = new List<Expression>();

                var parameter = Expression.Parameter(typeof(T), "o");

                string searchTerm = null;

                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    searchTerm = request.SearchTerm;
                    var properties = typeof(T).GetProperties()
                        .Where(c => c.GetCustomAttributes(typeof(NotMappedAttribute), false).Count() == 0)
                        .ToList();

                    if (properties.Any())
                    {
                        parameters = properties.Select(c =>
                        {
                            string key = null;

                            if (c.PropertyType.IsEnum || c.PropertyType.IsNullableEnum() || c.PropertyType == typeof(string))
                            {
                                key = c.Name + "|" + FilterType.contains.ToDescription() + "|searchterm";
                            }
                            else if (numberTypes.Contains(c.PropertyType) || c.PropertyType == typeof(double) || c.PropertyType == typeof(double?))
                            {
                                key = c.Name + "|" + FilterType.equals.ToDescription() + "|searchterm";
                            }
                            else if (c.PropertyType == typeof(DateTime) || c.PropertyType == typeof(DateTime?))
                            {
                                if (DateTime.TryParse(searchTerm, out DateTime filterValue))
                                {
                                    key = c.Name + "|" + FilterType.equals.ToDescription() + "|searchterm";
                                }
                            }

                            return new { Key = key, Value = searchTerm };

                        }).Where(c => !string.IsNullOrEmpty(c.Key))
                        .ToList();
                    }
                    else
                    {
                        throw new ArgumentNullException("No field found");
                    }

                }

                int pageSize = 10;
                int pageIndex = 1;
                var sortValue = new KeyValuePair<string, SortType>();

                if (request.PageSize <= 0)
                {
                    request.PageSize = 10;
                }

                if (request.PageIndex <= 0)
                {
                    request.PageIndex = 1;
                }

                pageSize = request.PageSize;
                pageIndex = request.PageIndex;

                //Find sort in QueryString
                if (!string.IsNullOrWhiteSpace(request.Sort))
                {
                    var order = request.Sort.Split(' ')[1];
                    sortValue = new KeyValuePair<string, SortType>(request.Sort.Split(' ')[0], order == "asc" ? SortType.Ascending : SortType.Descending);

                }

                foreach (var param in parameters)
                {
                    string propName = param.Key.Split('|')[0];
                    var filter = param.Key.Split('|').Count() > 1 ? param.Key.Split('|')[1] : null;
                    bool isSearchTerm = param.Key.Split('|').Count() > 2 ? param.Key.Split('|')[2] == "searchterm" : false;
                    var prop = typeof(T).GetProperties().FirstOrDefault(c => c.Name.ToLower().Equals(propName.ToLower()));

                    FilterType? filterType = null;
                    if (!string.IsNullOrEmpty(filter))
                    {
                        filterType = filters.FirstOrDefault(c => c.ToDescription() == filter);
                    }

                    if (prop != null)
                    {
                        var left = Expression.Property(parameter, prop);
                        Expression exp = null;

                        //Enum
                        if (prop.PropertyType.IsEnum || prop.PropertyType.IsNullableEnum())
                        {
                            var enumValues = Enum.GetValues(prop.PropertyType.IsNullableEnum() ? Nullable.GetUnderlyingType(prop.PropertyType) :
                                prop.PropertyType);

                            var listType = typeof(List<>);
                            var constructedListType = listType.MakeGenericType(prop.PropertyType);

                            var VCreator = Expression.Lambda<Func<System.Collections.IList>>(
                                Expression.New(constructedListType.GetConstructor(Type.EmptyTypes))
                                ).Compile();

                            var result = VCreator();

                            foreach (var val in enumValues)
                            {
                                var displayName = val.GetType().GetMember(val.ToString()).First().GetCustomAttribute<DisplayAttribute>()?.GetName();
                                if (displayName != null && displayName.Contains(param.Value.ToString()))
                                {
                                    result.Add(val);
                                }

                            }

                            if (result.Count > 0)
                            {
                                //Contains
                                var method = typeof(Enumerable).GetMember("Contains")[0] as MethodInfo;
                                var generic = method.MakeGenericMethod(prop.PropertyType);
                                var right = Expression.Constant(result);
                                exp = Expression.Call(null, generic, right, left);

                            }
                        }
                        // numberTypes
                        else if (numberTypes.Contains(prop.PropertyType))
                        {
                            if (long.TryParse(param.Value.ToString(), out long filterValue))
                            {
                                try
                                {
                                    var right = Expression.Constant(Convert.ChangeType(filterValue, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType));

                                    var rightExpression = Expression.Convert(right, prop.PropertyType);
                                    if (filterType == FilterType.equals)
                                    {
                                        exp = Expression.Equal(left, rightExpression);
                                    }

                                    if (filterType == FilterType.not_equals)
                                    {
                                        exp = Expression.NotEqual(left, rightExpression);
                                    }

                                    if (filterType == FilterType.less_then_or_equal)
                                    {
                                        exp = Expression.LessThanOrEqual(left, rightExpression);
                                    }

                                    if (filterType == FilterType.less_then)
                                    {
                                        exp = Expression.LessThan(left, rightExpression);
                                    }

                                    if (filterType == FilterType.greater_then)
                                    {
                                        exp = Expression.GreaterThan(left, rightExpression);
                                    }

                                    if (filterType == FilterType.greater_then_or_equal)
                                    {
                                        exp = Expression.GreaterThanOrEqual(left, rightExpression);
                                    }
                                }
                                catch (Exception)
                                {
                                }
                            }
                        }


                        // double And double?
                        else if (prop.PropertyType == typeof(double) || prop.PropertyType == typeof(double?))
                        {
                            if (double.TryParse(param.Value.ToString(), out double filterValue))
                            {
                                var right = Expression.Constant(Convert.ChangeType(filterValue, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType), prop.PropertyType);

                                if (filterType == FilterType.equals)
                                {
                                    exp = Expression.Equal(left, right);
                                }

                                if (filterType == FilterType.not_equals)
                                {
                                    exp = Expression.NotEqual(left, right);
                                }

                                if (filterType == FilterType.less_then_or_equal)
                                {
                                    exp = Expression.LessThanOrEqual(left, right);
                                }

                                if (filterType == FilterType.less_then)
                                {
                                    exp = Expression.LessThan(left, right);
                                }

                                if (filterType == FilterType.greater_then)
                                {
                                    exp = Expression.GreaterThan(left, right);
                                }

                                if (filterType == FilterType.greater_then_or_equal)
                                {
                                    exp = Expression.GreaterThanOrEqual(left, right);
                                }

                            }
                        }
                        else if (prop.PropertyType == typeof(decimal) || prop.PropertyType == typeof(decimal?))
                        {
                            if (double.TryParse(param.Value.ToString(), out double filterValue))
                            {
                                var right = Expression.Constant(Convert.ChangeType(filterValue, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType), prop.PropertyType);

                                if (filterType == FilterType.equals)
                                {
                                    exp = Expression.Equal(left, right);
                                }

                                if (filterType == FilterType.not_equals)
                                {
                                    exp = Expression.NotEqual(left, right);
                                }

                                if (filterType == FilterType.less_then_or_equal)
                                {
                                    exp = Expression.LessThanOrEqual(left, right);
                                }

                                if (filterType == FilterType.less_then)
                                {
                                    exp = Expression.LessThan(left, right);
                                }

                                if (filterType == FilterType.greater_then)
                                {
                                    exp = Expression.GreaterThan(left, right);
                                }

                                if (filterType == FilterType.greater_then_or_equal)
                                {
                                    exp = Expression.GreaterThanOrEqual(left, right);
                                }

                            }
                        }


                        // string
                        else if (prop.PropertyType == typeof(string))
                        {

                            var right = Expression.Constant(param.Value);
                            var loweredLeft = Expression.Call(left, typeof(string).GetMethod("ToLower", Type.EmptyTypes));


                            if (filterType == FilterType.contains)
                            {
                                var methods = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                                exp = Expression.Call(loweredLeft, methods, right);
                            }

                            if (filterType == FilterType.starts_with)
                            {
                                var methods = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
                                exp = Expression.Call(left, methods, right);
                            }

                            if (filterType == FilterType.ends_with)
                            {
                                var methods = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });
                                exp = Expression.Call(left, methods, right);
                            }

                            if (filterType == FilterType.not_equals)
                            {
                                exp = Expression.NotEqual(left, right);
                            }

                            if (filterType == FilterType.equals)
                            {
                                exp = Expression.Equal(left, right);
                            }

                        }

                        // DateTime And DateTime?
                        else if (prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(DateTime?))
                        {
                            if (DateTime.TryParse(param.Value.ToString(), out DateTime filterValue))
                            {
                                var rightStart = Expression.Convert(Expression.Constant(Convert.ChangeType(filterValue.Date, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType)), prop.PropertyType);
                                var rightEnd = Expression.Convert(Expression.Constant(Convert.ChangeType(filterValue.Date.AddDays(1), Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType)), prop.PropertyType);

                                if (filterType == FilterType.equals)
                                {
                                    exp = Expression.And(Expression.GreaterThanOrEqual(left, rightStart), Expression.LessThan(left, rightEnd));
                                }

                                if (filterType == FilterType.not_equals)
                                {
                                    exp = Expression.Not(Expression.And(Expression.GreaterThanOrEqual(left, rightStart), Expression.LessThan(left, rightEnd)));
                                }

                                if (filterType == FilterType.less_then_or_equal)
                                {
                                    exp = Expression.LessThanOrEqual(left, rightStart);
                                }

                                if (filterType == FilterType.less_then)
                                {
                                    exp = Expression.LessThan(left, rightStart);
                                }

                                if (filterType == FilterType.greater_then)
                                {
                                    exp = Expression.GreaterThan(left, rightStart);
                                }

                                if (filterType == FilterType.greater_then_or_equal)
                                {
                                    exp = Expression.GreaterThanOrEqual(left, rightStart);
                                }

                            }
                        }

                        // bool
                        else if (prop.PropertyType == typeof(bool) || prop.PropertyType == typeof(bool?))
                        {
                            if (bool.TryParse(param.Value.ToString(), out bool filterValue))
                            {
                                var right = Expression.Constant(Convert.ChangeType(filterValue, prop.PropertyType));


                                if (filterType == FilterType.equals)
                                {
                                    exp = Expression.Equal(left, right);
                                }

                                if (filterType == FilterType.not_equals)
                                {
                                    exp = Expression.NotEqual(left, right);

                                }
                            }
                        }

                        if (exp == null)
                        {
                            orExpressions.Add(Expression.Constant(false));
                        }
                        else
                        {
                            orExpressions.Add(exp);
                        }
                    }
                }

                // Apply Query Where
                if (orExpressions.Any())
                {
                    var finalOrExpression = orExpressions.Any() ? orExpressions.First() : Expression.Constant(true);
                    if (orExpressions.Count > 1)
                    {
                        for (int i = 1; i < orExpressions.Count; i++)
                        {
                            var expressions = orExpressions[i];
                            if (string.IsNullOrEmpty(request.SearchTerm))
                            {
                                finalOrExpression = Expression.AndAlso(finalOrExpression, expressions);
                            }
                            else
                            {
                                finalOrExpression = Expression.OrElse(finalOrExpression, expressions);

                            }
                        }
                    }

                    data = data.Where(Expression.Lambda<Func<T, bool>>(finalOrExpression, parameter));

                }
                //

                var total = data.Count();

                //Apply query Sort
                if (sortValue.Value > 0)
                {
                    IOrderedQueryable<T> tempData = null;

                    var prop2 = typeof(T).GetProperties().FirstOrDefault(c => c.Name.Equals(sortValue.Key, StringComparison.InvariantCultureIgnoreCase));

                    if (prop2 != null)
                    {

                        var parameterOrder = Expression.Parameter(typeof(T));
                        var lambda = Expression.Convert(Expression.Property(parameterOrder, prop2), typeof(object));
                        var expression = Expression.Lambda<Func<T, object>>(lambda, parameterOrder);

                        if (sortValue.Value == SortType.Ascending)
                        {
                            tempData = data.OrderBy(expression);
                        }
                        else
                        {
                            tempData = data.OrderByDescending(expression);

                        }

                    }

                    if (tempData != null)
                        data = tempData;

                }
                else
                {
                    // data = data.OrderBy(c=> true);
                }
                //

                //Paging
                if (pageSize == 0 && pageIndex == 1)
                {
                    // take all records
                }
                else if (pageSize > 0 && pageIndex > 0)
                {
                    data = ((IOrderedQueryable<T>)data).Skip((pageIndex - 1) * pageSize).Take(pageSize);
                }



                return new GridDto<T>
                {
                    List = await data.ToListAsync(cancellationToken),
                    Page = pageIndex,
                    Size = pageSize,
                    Total = total
                };

            }
            catch (Exception)
            {
                //Write Log
                //ex.Log();

                if (data != null)
                {
                    data = data.Where(c => false);
                }

                return new GridDto<T>
                {
                    List = await data.ToListAsync(cancellationToken)
                };
            }
        }



    }
}
