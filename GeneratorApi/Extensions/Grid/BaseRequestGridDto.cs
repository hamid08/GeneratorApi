namespace GeneratorApi.Extensions.Grid
{

    public class FilterModel
    {
        public object Key { get; set; }
        public object Value { get; set; }
    }

    public class BaseRequestGridDto
    {
        public List<FilterModel>? Filters { get; set; }=new List<FilterModel>();

        public bool ExcelExport { get; set; } = false;

        public string? SearchTerm { get; set; } = string.Empty;

        public int PageSize { get; set; }
        public int PageIndex { get; set; }

        public string? Sort { get; set; } = string.Empty;

    }
}