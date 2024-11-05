namespace project_prn231.Models
{
    public class PaginatedResponse<T>
    {
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public List<T> Items { get; set; }
    }

}
