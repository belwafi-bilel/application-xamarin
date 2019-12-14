
namespace RealEstate.Models.API.RealEstate
{
    public class RealEstateLoginLogDto
    {
        public long? LoginLogId { get; set; }
        public RealEstateAccountDto Account { get; set; }
        public string IPAddress { get; set; }
        public System.DateTime? CreateDate { get; set; }
        public bool? IsDelete { get; set; }
        

    }
}
