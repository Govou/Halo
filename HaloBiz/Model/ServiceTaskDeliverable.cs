namespace HaloBiz.Model
{
    public class ServiceTaskDeliverable : SetupBaseModel
    {
        public long ServiceCategoryTaskId { get; set; }
        public virtual ServiceCategoryTask ServiceCategoryTask { get; set; }
    }
}