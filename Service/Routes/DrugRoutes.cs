
namespace Service.Routes
{
    public static class DrugRoutes
    {
        public const string GetDrugById = "drugs/{id}";
        public const string GetDrugsCountBySearchTerm = "drugs/search/{keyword}";
        public const string GetDrugsBySearchTermPaged = "drugs/search/{keyword}/{page}/{amount}";
    }
}