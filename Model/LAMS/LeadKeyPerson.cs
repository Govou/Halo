namespace HaloBiz.Model.LAMS
{
    public class LeadKeyPerson : Contact
    {
        public long LeadId { get; set; }
        public virtual Lead Lead { get; set; }
        public long? CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}