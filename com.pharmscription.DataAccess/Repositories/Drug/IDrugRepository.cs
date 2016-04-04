
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using com.pharmscription.DataAccess.SharedInterfaces;

namespace com.pharmscription.DataAccess.Repositories.Drug
{
    public interface IDrugRepository : IRepository<Entities.DrugEntity.Drug>
    {
        Task<IEnumerable<DataAccess.Entities.DrugEntity.Drug>> SearchByName(string name);
        Task<DataAccess.Entities.DrugEntity.Drug> GetById(Guid id);

    }
}
