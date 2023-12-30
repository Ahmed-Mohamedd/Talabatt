namespace Talabat.Core.specifications
{
    public class ProductParams
    {
        const int MaxPageSize = 50;
        public int PageIndex { get; set; } = 1;
        private int pageSize = 5;

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value > MaxPageSize ? MaxPageSize : value ; }
        }

        public string? Sort { get; set; }
        public int? TypeId { get; set; }
        public int? BrandId { get; set; }

        private string? search;

        public string? Search
        {
            get { return search; }
            set { search = value.ToLower(); }
        }

    }
}
