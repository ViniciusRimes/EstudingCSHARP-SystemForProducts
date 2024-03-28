namespace APICatalog.Pagination
{
    public class ProductsFilteringForPrice : QueryStringParameters
    {
        public decimal? Price { get; set; }
        public string? PriceCritery { get; set; } //maior, menor ou igual

        
    }
}
