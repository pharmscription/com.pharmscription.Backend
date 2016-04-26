
namespace Service.Routes
{
    public static class DrugRoutes
    {
        public const string GetDrugById = "drugs/{id}";
        public const string GetDrugsBySearchTerm = "drugs/search/{keyword}";
        public const string GetDrugsCountBySearchTerm = "drugs/search/count/{keyword}";
        public const string GetDrugsBySearchTermPaged = "drugs/search/{keyword}/{page}/{amount}";
    }
}