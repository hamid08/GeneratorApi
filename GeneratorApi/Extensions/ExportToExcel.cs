using ExportExcelDynamicTest.CustomAttribute;
using GeneratorApi.Extensions.Grid;
using Microsoft.Net.Http.Headers;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace GeneratorApi.Extensions
{
    public class ExportToExcel<TModel>
    {
        public byte[] ExportToExcelFile(List<TModel> data)
        {

            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("Sheet1");
            sheet.IsRightToLeft = true;
            var cellStyle = workbook.CreateCellStyle();
            cellStyle.Alignment = HorizontalAlignment.Center;
            cellStyle.VerticalAlignment = VerticalAlignment.Center;

            PropertyInfo[] propertyInfoList = GetProperties();

            int columnCount = propertyInfoList.Length;

            // Add headers
            IRow headerRow = sheet.CreateRow(0);
            for (int i = 0; i < columnCount; i++)
            {
                ICell cell;

                PropertyInfo propertyInfo = propertyInfoList[i];

                var hasPlaqueNo = propertyInfo.GetCustomAttribute<ExcelExportHasPlaqueNoAttribute>();
                if (hasPlaqueNo != null)
                {
                    cell = headerRow.CreateCell(4);
                }
                else
                {
                    cell = headerRow.CreateCell(i);
                }

                if (i == 0)
                    cell.SetCellValue("ردیف");
                else
                {
                    string? headerName = (Attribute.GetCustomAttribute(propertyInfo, typeof(DisplayAttribute)) as DisplayAttribute)?.Name;

                    cell.SetCellValue(string.IsNullOrEmpty(headerName) ? propertyInfo.Name : headerName);

                }
            }

            // Add values
            int rowCount = 0;
            foreach (TModel entity in data)
            {
                rowCount++;
                IRow row = sheet.CreateRow(rowCount);

                for (int i = 0; i < columnCount; i++)
                {
                    ICell cell = row.CreateCell(i);
                    PropertyInfo propertyInfo = propertyInfoList[i];
                    PrepareValue(workbook, entity, cell, propertyInfo);
                }
            }

            using (var ms = new MemoryStream())
            {
                workbook.Write(ms);
                return ms.ToArray();
            }
        }

        private static void PrepareValue(IWorkbook workbook, TModel entity, ICell cell, PropertyInfo propertyInfo)
        {
            object value = propertyInfo.GetValue(entity)!;

            if (propertyInfo.PropertyType.IsEnum)
            {
                cell.SetCellValue(value != null ? ((Enum)value).ToDescription() : string.Empty);
            }
            else if (propertyInfo.PropertyType == typeof(double)
                || propertyInfo.PropertyType == typeof(double?))
            {
                cell.SetCellValue(value != null ? (double)value : 0);
            }
            else if (propertyInfo.PropertyType == typeof(float)
                || propertyInfo.PropertyType == typeof(float?))
            {
                cell.SetCellValue(value != null ? (float)value : 0);
            }
            else if (propertyInfo.PropertyType == typeof(int)
                || propertyInfo.PropertyType == typeof(int?))
            {
                cell.SetCellValue(value != null ? (int)value : 0);
            }
            else if (propertyInfo.PropertyType == typeof(long)
                || propertyInfo.PropertyType == typeof(long?))
            {
                cell.SetCellValue(value != null ? value.ToCurrencyFormat() : string.Empty);
            }
            else if (propertyInfo.PropertyType == typeof(bool)
                || propertyInfo.PropertyType == typeof(bool?))
            {
                var boolFa = propertyInfo.GetCustomAttribute<ExcelExportBoolFaAttribute>();
                if (boolFa != null)
                {
                    cell.SetCellValue(value != null ? (bool)value == true ? boolFa.TrueValue : boolFa.FalseValue : string.Empty);
                }
                else
                {
                    cell.SetCellValue(value != null ? (bool)value == true ? "دارد" : "ندارد" : string.Empty);
                }

            }
            else if (propertyInfo.PropertyType == typeof(DateTime)
                || propertyInfo.PropertyType == typeof(DateTime?))
            {
                cell.SetCellValue(value != null ? ((DateTime)value).ToPersian() : string.Empty);
            }
            else if (propertyInfo.PropertyType == typeof(TimeSpan)
                || propertyInfo.PropertyType == typeof(TimeSpan?))
            {
                cell.SetCellValue(value != null ? ((TimeSpan)value).ToString(@"hh\:mm") : string.Empty);
            }
            else
            {
                cell.SetCellValue(value != null ? value.ToString() : string.Empty);
            }
        }

        private static PropertyInfo[] GetProperties()
        {
            PropertyInfo[] propertyInfoList = typeof(TModel).GetProperties();
            List<PropertyInfo> properties = new List<PropertyInfo>();

            propertyInfoList.ToList().ForEach(propertyInfo =>
            {
                if (!IsNotMapped(propertyInfo))
                    properties.Add(propertyInfo);
            });

            return properties.ToArray();
        }

        private static bool IsNotMapped(PropertyInfo property)
        {
            return property.IsDefined(typeof(NotMappedAttribute), inherit: true);
        }
    }
}
