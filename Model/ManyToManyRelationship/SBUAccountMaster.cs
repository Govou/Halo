using HaloBiz.Model.AccountsModel;

namespace HaloBiz.Model.ManyToManyRelationship
{
    public class SBUAccountMaster
    {
        public long StrategicBusinessUnitId { get; set; }
        public StrategicBusinessUnit StrategicBusinessUnit { get; set; }
        public long AccountMasterId { get; set; }
        public AccountMaster AccountMaster { get; set; }
    }
}