namespace com.pharmscription.Infrastructure.Dto
{
    public class DrugDto: BaseDto
    {
        public string DrugDescription { get; set; }
        public string PackageSize { get; set; }
        public string Unit { get; set; }
        public string Composition { get; set; }
        public string NarcoticCategory { get; set; }
        public bool IsValid { get; set; }
        
    }
}
