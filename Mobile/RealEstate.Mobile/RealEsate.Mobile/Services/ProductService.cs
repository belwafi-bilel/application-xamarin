using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using RealEstate.Mobile.Services.SAL;
using RealEstate.Models.API.RealEstate;

namespace RealEstate.Mobile.Services
{
    public class ProductService : BaseService
    {
        protected override string Controller => "RealEstate";

        public async Task<EstateResultDto> GetProducts(ProductArguments productArguments)
        {
            var result = await Service.Post<EstateResultDto, NullServiceObject>(GenerateUrl("GetEstates"), productArguments);
            await HandleResponseShowError(result);
            return result?.Response?.Data;
        }
        public async Task<EstateResultDto> GetMyProducts(ProductArguments productArguments)
        {
            var result = await Service.Post<EstateResultDto, NullServiceObject>(GenerateUrl("GetMyEstates"), productArguments);
            await HandleResponseShowError(result);
            return result?.Response?.Data;
        }
        public async Task<MasterDataDto> GetMasterData()
        {
            var result = await Service.Get<MasterDataDto, NullServiceObject>(GenerateUrl("GetMasterData"));
            await HandleResponseShowError(result);
            return result?.Response?.Data;
        }
        public async Task<TownResultDto> GetTowns()
        {
            var result = await Service.Get<TownResultDto, NullServiceObject>(GenerateUrl("GetTowns"));
            await HandleResponseShowError(result);
            return result?.Response?.Data;
        }
        public async Task<Estate_TypeResultDto> GetListingTypes()
        {
            var result = await Service.Get<Estate_TypeResultDto, NullServiceObject>(GenerateUrl("GetEstate_Types"));
            await HandleResponseShowError(result);
            return result?.Response?.Data;
        }
        public async Task<SaleUnitResultDto> GetSaleUnits()
        {
            var result = await Service.Get<SaleUnitResultDto, NullServiceObject>(GenerateUrl("GetSaleUnits"));
            await HandleResponseShowError(result);
            return result?.Response?.Data;
        }
        public async Task<RentUnitResultDto> GetRentUnits()
        {
            var result = await Service.Get<RentUnitResultDto, NullServiceObject>(GenerateUrl("GetRentUnits"));
            await HandleResponseShowError(result);
            return result?.Response?.Data;
        }
        public async Task<long?> UploadMedias(string fileName, Stream stream)
        {
            var result = await Service.UploadFileStream<long, NullServiceObject>(GenerateUrl("UploadMedias"), fileName, stream);
            await HandleResponseShowError(result);
            return result?.Response?.Data ?? default(long?);
        }
        public async Task<bool> CreateProduct(RealEstateEstateDto product)
        {
            var result = await Service.Post<bool, NullServiceObject>(GenerateUrl("CreateEstate"), product);
            await HandleResponseShowError(result);
            return result?.Response?.Data ?? false;
        }
        public async Task<bool> UpdateProduct(RealEstateEstateDto product)
        {
            var result = await Service.Post<bool, NullServiceObject>(GenerateUrl("UpdateEstate"), product);
            await HandleResponseShowError(result);
            return result?.Response?.Data ?? false;
        }
        public async Task<bool> ChangePassword(RealEstateAccountChangePasswordDto realEstateAccount)
        {
            var result = await Service.Post<bool, NullServiceObject>(GenerateUrl("ChangePassword"), realEstateAccount);
            await HandleResponseShowError(result);
            return result?.Response?.Data ?? false;
        }

        public async Task<bool> UpdateDeleteProduct(RealEstateDeleteEstateDto arguments)
        {
            var result = await Service.Post<bool, NullServiceObject>(GenerateUrl("UpdateDeleteEstate"), arguments);
            await HandleResponseShowError(result);
            return result?.Response?.Data ?? false;
        }
        public async Task<RealEstateDashboardDto> GetDashBoard()
        {
            var result = await Service.Get<RealEstateDashboardDto, NullServiceObject>(GenerateUrl("GetDashBoard"));
            await HandleResponseIgnoreError(result);
            return result?.Response?.Data;
        }
    }
}
