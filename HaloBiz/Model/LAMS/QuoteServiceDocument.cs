namespace HaloBiz.Model.LAMS
{
    public class QuoteServiceDocument : Documents
    {
        public string Type { get; set; }
        public long QuoteServiceId { get; set; }
        public QuoteService QuoteService { get; set; }
    }
}