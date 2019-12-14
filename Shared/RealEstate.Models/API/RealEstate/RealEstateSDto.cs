
using System.Collections.Generic;

namespace RealEstate.Models.API.RealEstate
{
    public class RealEstateEstateDto
    {
        public long Id { get; set; }
        public int? Estate_TypeId { get; set; }
        public string NameEstate_Type { get; set; }
        public RealEstateAccountDto Account { get; set; }
        public RealEstateTownDto Town { get; set; }
        public int? SaleOrRent { get; set; }
        public string EstateCode { get; set; }
        public string Area { get; set; }
        public double? SalePrice { get; set; }
        public RealEstateSaleUnitDto SaleUnit{ get; set; }
        public double? RentPrice { get; set; }
        public RealEstateRentUnitDto RentUnit { get; set; }
        public string HouseNumber { get; set; }
        public string LotCode { get; set; }
        public string Phone { get; set; }
        public string OwnerName { get; set; }
        public System.DateTime? CreatedDate { get; set; }
        public System.DateTime? ModifiedDate { get; set; }
        public string Note { get; set; }
        public bool? IsHot { get; set; }
        public string LogBy { get; set; }
        public bool? IsDelete { get; set; }
        public double ? Lat { get; set; }
        public double ? Long { get; set; }
        public string MainPinText { get; set; }
        public string Address { get; set; }
        public List<long> EstateImageIds { get; set; } = new List<long>();
        public List<EstateImageDto> ImageUrls { get; set; }

    }
    public class EstateImageDto
    {
        public long Id { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
    }
    public class RealEstateDeleteEstateDto
    {
        public long Id { get; set; }       
        public long AccountId { get; set; }
        public bool? IsDelete { get; set; }

    }
}
