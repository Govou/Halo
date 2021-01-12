namespace HaloBiz.Model.ManyToManyRelationship
{
    public class ServiceRequiredServiceField
    {
        public long ServicesId { get; set; }
        public Services Services { get; set; }
        public long RequiredServiceFieldId { get; set; }
        public long RequiredServiceField { get; set; }

    }
}