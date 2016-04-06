using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using com.pharmscription.Infrastructure.Dto;

namespace com.pharmscription.BusinessLogic.Drug
{
    public interface IDrugManager
    {
        Task<List<DrugDto>> Search(string partialDescription);
        Task<DrugDto> Add(DrugDto drug);
        Task<DrugDto> Edit(DrugDto drug);
        Task<DrugDto> GetById(string id);
        Task<DrugDto> RemoveById(string id);
    }
}
