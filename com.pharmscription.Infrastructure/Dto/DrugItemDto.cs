namespace com.pharmscription.Infrastructure.Dto
{
    public class DrugItemDto : BaseDto
    {
        public DrugDto Drug { get; set; }
        public string DosageDescription { get; set; }
        public int Quantity { get; set; }
    }

}
