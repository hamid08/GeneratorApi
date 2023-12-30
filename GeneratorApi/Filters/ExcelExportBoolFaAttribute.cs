namespace ExportExcelDynamicTest.CustomAttribute
{
    public class ExcelExportBoolFaAttribute : Attribute
    {
        public ExcelExportBoolFaAttribute(string trueValue, string falseValue)
        {
            TrueValue = trueValue;
            FalseValue = falseValue;
        }

        public string TrueValue { get; }
        public string FalseValue { get; }
    }
}
