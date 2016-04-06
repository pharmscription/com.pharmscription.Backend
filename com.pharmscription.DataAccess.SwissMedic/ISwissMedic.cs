using System.Threading.Tasks;
using com.pharmscription.DataAccess.Entities.DrugEntity;

namespace com.pharmscription.DataAccess.SwissMedic
{
    public interface ISwissMedic
    {
        Task<Drug> SearchDrug(string partialDescription);
        Task<Drug> GetAll();
    }
}
