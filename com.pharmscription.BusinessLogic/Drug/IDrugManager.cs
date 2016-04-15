using System.Collections.Generic;
using System.Threading.Tasks;
using com.pharmscription.Infrastructure.Dto;

namespace com.pharmscription.BusinessLogic.Drug
{
    public interface IDrugManager
    {
        Task<int> Search(string partialDescription);
        Task<List<DrugDto>> SearchPaged(string partialDescription, string pageNumber, string amountPerPage);
        Task<DrugDto> GetById(string id);
    }
}
