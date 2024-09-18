namespace SplitKaro.Helpers
{
    public class QueryObject
    {
        public string? GroupName { get; set; }

        public string? CreatedBy { get; set; }

        public string? SortBy { get; set; }

        public bool IsDecending { get; set; }

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}
