
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using com.pharmscription.BusinessLogic.Converter;
using com.pharmscription.DataAccess.Repositories.Drug;
using com.pharmscription.Infrastructure.Dto;

namespace com.pharmscription.BusinessLogic.Drug
{
    public class DrugManager : CoreWorkflow, IDrugManager
    {
        private readonly IDrugRepository _repository;
        private readonly SwissMedicConnector _swissMedicConnector;

        public DrugManager(Context context, IDrugRepository repository) : base(context)
        {
            _repository = repository;
            _swissMedicConnector = new SwissMedicConnector();
        }

        public async Task<List<DrugDto>> Search(string partialDescription)
        {
            if (partialDescription == null)
            {
                throw new ArgumentNullException(nameof(partialDescription));
            }
            if (string.IsNullOrWhiteSpace(partialDescription))
            {
                throw new ArgumentException("Search Param was empty");
            }
            var drugsCachesLocally = await _repository.SearchByName(partialDescription);
            if (drugsCachesLocally.Any())
            {
                return drugsCachesLocally.ConvertToDtos();
            }
            var drugsFromSwissMedic =
                await _swissMedicConnector.GetSwissMedicConnection().SearchDrug(partialDescription);
            await CacheSwissMedicLocally(drugsFromSwissMedic);
            return drugsFromSwissMedic.ConvertToDtos();
        }

        private async Task CacheSwissMedicLocally(IEnumerable<DataAccess.Entities.DrugEntity.Drug> drugsFromSwissMedic)
        {
            foreach (var drug in drugsFromSwissMedic)
            {
                _repository.Add(drug);
            }
            await _repository.UnitOfWork.CommitAsync();
        }


        public async Task<DrugDto> Add(DrugDto drug)
        {
            if (drug == null)
            {
                throw new ArgumentNullException(nameof(drug));
            }
            _repository.Add(drug.ConvertToEntity());
            await _repository.UnitOfWork.CommitAsync();
            return drug;
        }

        public async Task<DrugDto> Edit(DrugDto drug)
        {
            if (drug == null)
            {
                throw new ArgumentNullException(nameof(drug));
            }
            if (string.IsNullOrWhiteSpace(drug.Id))
            {
                throw new ArgumentException("No Entity with such an Id was found in Database");
            }
            var drugInDatabase = await _repository.GetAsync(new Guid(drug.Id));
            if (drugInDatabase == null)
            {
                throw new ArgumentException("No Entity with such an Id was found in Database");
            }
            drugInDatabase.DrugDescription = drug.DrugDescription;
            drugInDatabase.Composition = drug.Composition;
            drugInDatabase.IsValid = drug.IsValid;
            drugInDatabase.NarcoticCategory = drug.NarcoticCategory;
            drugInDatabase.PackageSize = drug.PackageSize;
            drugInDatabase.Unit = drug.Unit;
            await _repository.UnitOfWork.CommitAsync();
            return drugInDatabase.ConvertToDto();
        }

        public async Task<DrugDto> GetById(string id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("Id was empty");
            }
            return (await _repository.GetAsync(new Guid(id))).ConvertToDto();
        }

        public async Task<DrugDto> RemoveById(string id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("Id was empty");
            }
            var drug = await _repository.GetAsync(new Guid(id));
            if (drug != null)
            {
                _repository.Remove(drug);
                await _repository.UnitOfWork.CommitAsync();
            }
            return drug.ConvertToDto();
        }
    }
}
