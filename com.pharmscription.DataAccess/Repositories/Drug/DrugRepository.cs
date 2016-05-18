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

        public async Task<List<Entities.DrugEntity.Drug>> SearchByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new InvalidArgumentException("Search Param was empty or null");
            }
            var searchText = name.ToLower();
            return await Set.Where(e => e.DrugDescription.ToLower().Contains(searchText)).ToListAsync();
        }

        public async Task<List<Entities.DrugEntity.Drug>> SearchByNamePaged(string name, int pageNumber, int amountPerPage)
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
            var drugsFitting = await Set.Where(e => e.DrugDescription.ToLower().Contains(searchText)).ToListAsync();
            var drugsOrdered = drugsFitting.OrderBy(e => e.DrugDescription);
            var drugsWithoutSkipped = drugsOrdered.Skip(pageNumber*amountPerPage);
            var drugsSelected = drugsWithoutSkipped.Take(amountPerPage);
            return drugsSelected.ToList();
        }
    }
}
