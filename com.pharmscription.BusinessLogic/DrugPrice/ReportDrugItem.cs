namespace com.pharmscription.BusinessLogic.DrugPrice
{
    public class ReportDrugItem
    {
        public int Quantity { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }

        public string FormattedPrice()
        {
            return Price.ToString("F");
        }

        public string TotalPrice()
        {
            return (Quantity * Price).ToString("F");
        }
    }
}
