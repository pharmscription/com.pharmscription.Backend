using System.Collections.Generic;
using System.Threading.Tasks;
using com.pharmscription.Infrastructure.Dto;

namespace com.pharmscription.BusinessLogic.Drug
{
    public interface IDrugManager
    {
        Task<List<DrugDto>> Search(string partialDescription);
        Task<List<DrugDto>> SearchPaged(string partialDescription, int pageNumber, int amountPerPage);
        Task<DrugDto> GetById(string id);
    }
}
