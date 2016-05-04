namespace com.pharmscription.Infrastructure.Dto
{
    public class DoctorDto : BaseDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserId { get; set; }
        public AddressDto Address { get; set; }
        // ReSharper disable once InconsistentNaming
        public string ZSRNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
    }
}