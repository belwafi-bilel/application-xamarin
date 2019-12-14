using RealEstate.Models.API.RealEstate;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;

namespace RealEstate.API.SAL.RealEstateService
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRealEstateService
    {
        /// <summary>
        /// Account
        /// </summary>
        bool UpdateDeleteEstate(RealEstateDeleteEstateDto model);
        RealEstateDashboardDto GetDashBoard();
        List<string> GetRoleNameByAccountId(long accountId);
        LoginModel GetLoginByEmail(string Email);
        List<RealEstateAccountDto> GetAllAccounts();
        bool UpdateAccount(RealEstateAccountDto model);
        bool CreateAccount(RealEstateAccountDto model);

        List<RealEstateDepartmentDto> GetAllDepartments();
        EstateResultDto GetAllEstates(ProductArguments productArguments);
        EstateResultDto GetAllMyEstates(ProductArguments productArguments);
        Task<long?> UploadFile(HttpPostedFile file);
        bool CreateEstate(RealEstateEstateDto model);
        bool UpdateEstate(RealEstateEstateDto model);
        
        List<RealEstateTownDto> GetAllTowns();
        List<RealEstateRentUnitDto> GetAllRentUnits();
        List<RealEstateSaleUnitDto> GetAllSaleUnits();
        List<RealEstateEstate_TypeDto> GetAllEstate_Types();
        List<RealEstateLoginLogDto> GetAllLoginLogs(int page, int pageSize);
        bool CreateLoginLog(RealEstateLoginLogDto model);
        bool ChangePassword(RealEstateAccountChangePasswordDto model);
    }
}