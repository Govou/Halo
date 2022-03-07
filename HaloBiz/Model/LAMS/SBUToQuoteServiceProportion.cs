namespace HaloBiz.Model.LAMS
{
    public class SBUToQuoteServiceProportion : QuoteServiceProportion
    {
        public long QuoteServiceId { get; set; }
        public QuoteService QuoteService { get; set; }
    }
}