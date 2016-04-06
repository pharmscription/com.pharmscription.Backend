
using System.Collections.Generic;
using System.Threading.Tasks;
using com.pharmscription.DataAccess.Repositories.Drug;
using com.pharmscription.Infrastructure.Dto;

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

        public Task<List<DrugDto>> Search(string partialDescription)
        {
            throw new System.NotImplementedException();
        }

        public Task<DrugDto> Add(DrugDto drug)
        {
            throw new System.NotImplementedException();
        }

        public Task<DrugDto> Edit(DrugDto drug)
        {
            throw new System.NotImplementedException();
        }

        public Task<DrugDto> GetById(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<DrugDto> RemoveById(string id)
        {
            throw new System.NotImplementedException();
        }
    }
}
