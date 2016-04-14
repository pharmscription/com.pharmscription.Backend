using System.Collections.Generic;
using System.Threading.Tasks;
using com.pharmscription.DataAccess.Entities.AccountEntity;
using com.pharmscription.Infrastructure.Dto;
using com.pharmscription.Security;

namespace com.pharmscription.BusinessLogic.Drug
{
    public interface IDrugManager
    {
        [Authorized(Role = Role.Doctor)]
        Task<List<DrugDto>> Search(string partialDescription);
        [Authorized(Role = Role.Doctor)]
        Task<List<DrugDto>> SearchPaged(string partialDescription, int pageNumber, int amountPerPage);
        [Authorized(Role = Role.Doctor)]
        Task<DrugDto> GetById(string id);
    }
}
