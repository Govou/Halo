namespace HaloBiz.Model.ManyToManyRelationship
{
    public class ServiceRequiredServiceDocument
    {
        public long ServicesId { get; set; }
        public Services Services { get; set; }
        public long RequiredServiceDocumentId { get; set; }
        public RequiredServiceDocument RequiredServiceDocument { get; set; }
    }
}