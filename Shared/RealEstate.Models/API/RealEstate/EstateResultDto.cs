using RealEstate.Models.API.RealEstate;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Models.API.RealEstate
{
    public class EstateResultDto
    {
        public List<RealEstateEstateDto> EstateResults { get; set; }
        public int TotalRecord { get; set; }
    }
    public class MasterDataDto
    {
        public MasterDataResult MasterData { get; set; } = new MasterDataResult();
    }
    public class MasterDataResult
    {
        public List<RealEstateSaleUnitDto> SaleUnits { get; set; }
        public List<RealEstateRentUnitDto> RentUnits { get; set; }
        public List<RealEstateEstate_TypeDto> ListingTypes { get; set; }
        public List<RealEstateTownDto> Towns { get; set; }
    }
}
