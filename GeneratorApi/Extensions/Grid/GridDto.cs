namespace GeneratorApi.Extensions.Grid
{
    public class GridDto<T> where T : class
    {
        public List<T> List { get; set; }
        public int Page { get; set; }
        public int Size { get; set; }
        public int Total { get; set; }
    }
}