using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using com.pharmscription.DataAccess.Repositories.BaseRepository;
using com.pharmscription.DataAccess.UnitOfWork;

namespace com.pharmscription.DataAccess.Repositories.Drug
{
    public class DrugRepository : Repository<Entities.DrugEntity.Drug>, IDrugRepository
    {
        public DrugRepository(IPharmscriptionUnitOfWork pharmscriptionUnitOfWork) : base(pharmscriptionUnitOfWork)
        {
            
        }

        public Task<List<Entities.DrugEntity.Drug>> SearchByName(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Search Param was empty");
            }
            var searchText = name.ToLower();
            return GetSet().Where(e => e.DrugDescription.ToLower().Contains(searchText)).ToListAsync();
        }
    }
}
