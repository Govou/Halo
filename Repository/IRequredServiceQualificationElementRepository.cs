using System.Collections.Generic;
using System.Threading.Tasks;
using HaloBiz.Model;

namespace HaloBiz.Repository
{
    public interface IRequredServiceQualificationElementRepository
    {
        Task<RequredServiceQualificationElement> SaveRequredServiceQualificationElement(RequredServiceQualificationElement RequredServiceQualificationElement);

        Task<RequredServiceQualificationElement> FindRequredServiceQualificationElementById(long Id);

        Task<RequredServiceQualificationElement> FindRequredServiceQualificationElementByName(string name);

        Task<IEnumerable<RequredServiceQualificationElement>> FindAllRequredServiceQualificationElements();

        Task<RequredServiceQualificationElement> UpdateRequredServiceQualificationElement(RequredServiceQualificationElement RequredServiceQualificationElement);

        Task<bool> DeleteRequredServiceQualificationElement(RequredServiceQualificationElement RequredServiceQualificationElement);
        Task<bool> DeleteRequredServiceQualificationElementRange(IEnumerable<RequredServiceQualificationElement> RequredServiceQualificationElements);
    }
}
