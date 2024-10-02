namespace Repository.Model
{
    public class ResponseSearchModel<T>
    {
        public IEnumerable<T>? Entities { get; set; }
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
        public int? TotalPages { get; set; }
        public int? TotalItems { get; set; }
    }
}
