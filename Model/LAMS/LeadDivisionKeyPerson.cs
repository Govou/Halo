namespace HaloBiz.Model.LAMS
{
    public class LeadDivisionKeyPerson : Contact
    {
        public long LeadDivisionId { get; set; }
        public virtual LeadDivision LeadDivision { get; set; }
        public long? CustomerDivisionId { get; set; }
        public CustomerDivision CustomerDivision { get; set; }
    }
}