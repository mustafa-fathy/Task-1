namespace Common
{
    public class PaginatedList<T>
    {
        public object Items { get; set; }
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public PaginatedList(object items, int totalcount, int pagenumber, int pagesize, int totalpages)
        {
            Items = items;
            TotalCount = totalcount;
            PageNumber = pagenumber;
            PageSize = pagesize;
            TotalPages = pagesize;

        }
        public PaginatedList()
        {

        }
    }
}
