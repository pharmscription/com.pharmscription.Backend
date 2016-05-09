
namespace com.pharmscription.Service.Routes
{
    public static class DrugRoutes
    {
        internal const string GetDrugById = "drugs/{id}";
        internal const string GetDrugsCountBySearchTerm = "drugs/search/count/{keyword}";
        internal const string GetDrugsBySearchTermPaged = "drugs/search/{keyword}/{page}/{amount}";
    }
}