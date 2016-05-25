namespace com.pharmscription.DataAccess.Repositories.DrugItem
{
    using System;
    using System.Threading.Tasks;

    using Entities.DrugItemEntity;
    using SharedInterfaces;

    /// <summary>
    /// The DrugItemRepository interface.
    /// </summary>
    public interface IDrugItemRepository : IRepository<DrugItem>
    {
        Task<DrugItem> GetWithDrugAsync(Guid drugItemId);
    }
}