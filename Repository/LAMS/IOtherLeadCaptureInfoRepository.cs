using System.Collections.Generic;
using System.Threading.Tasks;
using HaloBiz.Model.LAMS;

namespace HaloBiz.Repository.LAMS
{
    public interface IOtherLeadCaptureInfoRepository
    {
        Task<OtherLeadCaptureInfo> SaveOtherLeadCaptureInfo(OtherLeadCaptureInfo otherLeadCaptureInfo);
        Task<OtherLeadCaptureInfo> FindOtherLeadCaptureInfoById(long Id);
        Task<IEnumerable<OtherLeadCaptureInfo>> FindAllOtherLeadCaptureInfo();
        Task<OtherLeadCaptureInfo> UpdateOtherLeadCaptureInfo(OtherLeadCaptureInfo otherLeadCaptureInfo);
        Task<bool> DeleteOtherLeadCaptureInfo(OtherLeadCaptureInfo otherLeadCaptureInfo);
    }
}