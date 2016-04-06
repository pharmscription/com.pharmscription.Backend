using System.Collections.Generic;
using System.Threading.Tasks;
using com.pharmscription.DataAccess.SharedInterfaces;

namespace com.pharmscription.DataAccess.Repositories.Drug
{
    public interface IDrugRepository : IRepository<Entities.DrugEntity.Drug>
    {
        Task<List<Entities.DrugEntity.Drug>> SearchByName(string name);
    }
}
