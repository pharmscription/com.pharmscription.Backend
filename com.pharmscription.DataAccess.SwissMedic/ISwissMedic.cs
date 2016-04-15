using System.Collections.Generic;
using System.Threading.Tasks;
using com.pharmscription.DataAccess.Entities.DrugEntity;

namespace com.pharmscription.DataAccess.SwissMedic
{
    public interface ISwissMedic
    {
        Task<List<Drug>> SearchDrug(string partialDescription);
        Task<List<Drug>> GetAll();
    }
}
