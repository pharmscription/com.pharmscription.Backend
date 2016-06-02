using System.Collections.Generic;

namespace com.pharmscription.Infrastructure.Dto
{
    public class DispenseDto  : BaseDto
    {
        public string Date { get; set; }
        public string Remark { get; set; }
        public ICollection<DrugItemDto> DrugItems { get; set; }
    }
}
