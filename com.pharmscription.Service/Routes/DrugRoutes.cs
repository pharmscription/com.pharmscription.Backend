
namespace com.pharmscription.Service.Routes
{
    public class DrugRoutes
    {
        protected internal const string GetDrugById = "drugs/{id}";
        protected internal const string GetDrugsCountBySearchTerm = "drugs/search/count/{keyword}";
        protected internal const string GetDrugsBySearchTermPaged = "drugs/search/{keyword}/{page}/{amount}";
    }
}