using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using com.pharmscription.DataAccess.Repositories.BaseRepository;
using com.pharmscription.DataAccess.UnitOfWork;
using com.pharmscription.Infrastructure.Exception;

namespace com.pharmscription.DataAccess.Repositories.Drug
{
    public class DrugRepository : Repository<Entities.DrugEntity.Drug>, IDrugRepository
    {
        public DrugRepository(IPharmscriptionUnitOfWork pharmscriptionUnitOfWork) : base(pharmscriptionUnitOfWork)
        {
            
        }

        public Task<List<Entities.DrugEntity.Drug>> SearchByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new InvalidArgumentException("Search Param was empty or null");
            }
            var searchText = name.ToLower();
            return GetSet().Where(e => e.DrugDescription.ToLower().Contains(searchText)).ToListAsync();
        }

        public Task<List<Entities.DrugEntity.Drug>> SearchByNamePaged(string name, int pageNumber, int amountPerPage)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new InvalidArgumentException("Search Param was empty or null");
            }
            if (pageNumber < 0 || amountPerPage < 0)
            {
                throw new InvalidArgumentException("Negative Pagenumber or amountperPage supplied");
            }
            var searchText = name.ToLower();
            return GetSet().Where(e => e.DrugDescription.ToLower().Contains(searchText)).OrderBy(e => e.DrugDescription).Skip(pageNumber * amountPerPage).Take(amountPerPage).ToListAsync();
        }
    }
}
