
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using com.pharmscription.BusinessLogic.Converter;
using com.pharmscription.DataAccess.Repositories.Drug;
using com.pharmscription.Infrastructure.Dto;
using com.pharmscription.Infrastructure.Exception;

namespace com.pharmscription.BusinessLogic.Drug
{
    public class DrugManager : IDrugManager
    {
        private readonly IDrugRepository _repository;
        private readonly SwissMedicConnector _swissMedicConnector;

        public DrugManager(IDrugRepository repository)
        {
            _repository = repository;
            _swissMedicConnector = new SwissMedicConnector();
        }

        public async Task<List<DrugDto>> SearchPaged(string partialDescription, int pageNumber, int amountPerPage)
        {
            if (string.IsNullOrWhiteSpace(partialDescription))
            {
                throw new InvalidArgumentException("Search Param was empty or null");
            }
            if (pageNumber < 0 || amountPerPage < 0)
            {
                throw new InvalidArgumentException("Negative pagenumber or amountperpage supplied");
            }
            var drugsAreCachedLocally = _repository.GetAll().Any();
            if (!drugsAreCachedLocally)
            {
                await LoadDrugsFromSwissMedic();
            }
            var drugsFittingSearch = await _repository.SearchByNamePaged(partialDescription, pageNumber, amountPerPage);

            return drugsFittingSearch.ConvertToDtos();
        }

        public async Task<List<DrugDto>> Search(string partialDescription)
        {
            if (string.IsNullOrWhiteSpace(partialDescription))
            {
                throw new InvalidArgumentException("Search Param was empty or null");
            }
            var drugsAreCachedLocally = _repository.GetAll().Any();
            if (!drugsAreCachedLocally)
            {
                await LoadDrugsFromSwissMedic();
            }
            var drugsFittingSearch = await _repository.SearchByName(partialDescription);
            return drugsFittingSearch.ConvertToDtos();
        }

        private async Task LoadDrugsFromSwissMedic()
        {
            var drugsFromSwissMedic = (await _swissMedicConnector.GetSwissMedicConnection().GetAll()).ToList();
            await CacheSwissMedicLocally(drugsFromSwissMedic);
        }

        private async Task CacheSwissMedicLocally(IEnumerable<DataAccess.Entities.DrugEntity.Drug> drugsFromSwissMedic)
        {
            foreach (var drug in drugsFromSwissMedic)
            {
                _repository.Add(drug);
            }
            await _repository.UnitOfWork.CommitAsync();
        }

        public async Task<DrugDto> GetById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new InvalidArgumentException("Id was empty");
            }
            return (await _repository.GetAsync(new Guid(id))).ConvertToDto();
        }
    }
}
