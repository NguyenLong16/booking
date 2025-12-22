namespace BookingSystem.API.DTOs
{
    public class SearchFilterDto
    {
        public string? Keyword { get; set;  }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
    }
}
