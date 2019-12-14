using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using RealEstate.Models.API.RealEstate;
using RealEstate.API.SAL.RealEstateService;
using RealEstate.Models.API;
using RealEstate.API.Filters;
using System.Web;

namespace RealEstate.API.Controllers
{
    [Authorize]
    [RoutePrefix("api/RealEstate")]
    public class RealEstateApiController : BaseApiController
    {
        private readonly IRealEstateService _service;

        public RealEstateApiController(IRealEstateService service)
        {
            _service = service;
        }


        [HttpGet]
        [JwtAuthentication]
        [Route(nameof(GetDashBoard)), ResponseType(typeof(RealEstateResponse<RealEstateDashboardDto>))]
        public async Task<IHttpActionResult> GetDashBoard()
        {
            RealEstateDashboardDto result = new RealEstateDashboardDto();
            result = _service.GetDashBoard();
            return Ok(RealEstateResponse<RealEstateDashboardDto>.Create(result));
        }
        #region Account

       
        [HttpGet]
        [JwtAuthentication]
        [Route(nameof(GetAccounts)), ResponseType(typeof(RealEstateResponse<AccountResultDto>))]
        public async Task<IHttpActionResult> GetAccounts()
        {
            AccountResultDto result = new AccountResultDto();
            List<RealEstateAccountDto> accounts = _service.GetAllAccounts();
            result.AccountResults = accounts;
            return Ok(RealEstateResponse<AccountResultDto>.Create(result));
        }

        /// <summary>
        /// Update Account of current logged user
        /// </summary>
        /// <returns>Return Account</returns>
        [HttpPost]
        [JwtAuthentication]
        [Route(nameof(UpdateAccount)), ResponseType(typeof(RealEstateResponse<bool>))]
        public async Task<IHttpActionResult> UpdateAccount(RealEstateAccountDto arguments)
        {
            bool update = _service.UpdateAccount(arguments);

            return Ok(RealEstateResponse<bool>.Create(update));
        }
        /// <summary>
        /// Update Password Account of current logged user
        /// </summary>
        /// <returns>Return bool</returns>
        [HttpPost]
        [JwtAuthentication]
        [Route(nameof(ChangePassword)), ResponseType(typeof(RealEstateResponse<bool>))]
        public async Task<IHttpActionResult> ChangePassword(RealEstateAccountChangePasswordDto arguments)
        {
            bool update = _service.ChangePassword(arguments);

            return Ok(RealEstateResponse<bool>.Create(update));
        }
        #endregion

        #region Department      
        [HttpGet]
        [JwtAuthentication]
        [Route(nameof(GetDepartments)), ResponseType(typeof(RealEstateResponse<DepartmentResultDto>))]
        public async Task<IHttpActionResult> GetDepartments()
        {
            DepartmentResultDto result = new DepartmentResultDto();
            List<RealEstateDepartmentDto> departments = _service.GetAllDepartments();
            result.DepartmentResults = departments;
            return Ok(RealEstateResponse<DepartmentResultDto>.Create(result));
        }
        #endregion

        #region Estate

        /// <summary>
        /// Estate API
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpPost]
        [JwtAuthentication]
        [Route(nameof(GetEstates)), ResponseType(typeof(RealEstateResponse<EstateResultDto>))]
        public async Task<IHttpActionResult> GetEstates(ProductArguments productArguments)
        {   
            var result = _service.GetAllEstates(productArguments);
            return Ok(RealEstateResponse<EstateResultDto>.Create(result));
        }


        [HttpPost]
        [JwtAuthentication]
        [Route(nameof(GetMyEstates)), ResponseType(typeof(RealEstateResponse<EstateResultDto>))]
        public async Task<IHttpActionResult> GetMyEstates(ProductArguments productArguments)
        {
            var result = _service.GetAllMyEstates(productArguments);
            return Ok(RealEstateResponse<EstateResultDto>.Create(result));
        }
        /// <summary>
        /// Update Estate of current logged user
        /// </summary>
        /// <returns>Return bool</returns>
        [HttpPost]
        [JwtAuthentication]
        [Route(nameof(UpdateEstate)), ResponseType(typeof(RealEstateResponse<bool>))]
        public async Task<IHttpActionResult> UpdateEstate(RealEstateEstateDto arguments)
        {
            bool update = _service.UpdateEstate(arguments);

            return Ok(RealEstateResponse<bool>.Create(update));
        }

        [HttpPost]
        [JwtAuthentication]
        [Route(nameof(UploadMedias)), ResponseType(typeof(RealEstateResponse<long?>))]
        public async Task<IHttpActionResult> UploadMedias()
        {
            var file = HttpContext.Current.Request.Files.Count > 0 ? HttpContext.Current.Request.Files[0] : null;

            var result = await _service.UploadFile(file);
            return Ok(RealEstateResponse<long?>.Create(result));
        }

        /// <summary>
        /// Update Estate isDelete of current logged user
        /// </summary>
        /// <returns>Return bool</returns>
        [HttpPost]
        [JwtAuthentication]
        [Route(nameof(UpdateDeleteEstate)), ResponseType(typeof(RealEstateResponse<bool>))]
        public async Task<IHttpActionResult> UpdateDeleteEstate(RealEstateDeleteEstateDto arguments)
        {
            bool update = _service.UpdateDeleteEstate(arguments);

            return Ok(RealEstateResponse<bool>.Create(update));
        }
        /// <summary>
        /// Create Estate of current logged user
        /// </summary>
        /// <returns>Return bool</returns>
        [HttpPost]
        [JwtAuthentication]
        [Route(nameof(CreateEstate)), ResponseType(typeof(RealEstateResponse<bool>))]
        public async Task<IHttpActionResult> CreateEstate(RealEstateEstateDto arguments)
        {
            bool update = _service.CreateEstate(arguments);

            return Ok(RealEstateResponse<bool>.Create(update));
        }
        #endregion

        #region GetAll
        [HttpGet]
        [AllowAnonymous]
        [Route(nameof(GetSaleUnits)), ResponseType(typeof(RealEstateResponse<SaleUnitResultDto>))]
        public async Task<IHttpActionResult> GetSaleUnits()
        {
            SaleUnitResultDto result = new SaleUnitResultDto();
            List<RealEstateSaleUnitDto> SaleUnits = _service.GetAllSaleUnits();
            result.SaleUnitResults = SaleUnits;
            return Ok(RealEstateResponse<SaleUnitResultDto>.Create(result));
        }
        [HttpGet]
        [AllowAnonymous]
        [Route(nameof(GetRentUnits)), ResponseType(typeof(RealEstateResponse<RentUnitResultDto>))]
        public async Task<IHttpActionResult> GetRentUnits()
        {
            RentUnitResultDto result = new RentUnitResultDto();
            List<RealEstateRentUnitDto> RentUnits = _service.GetAllRentUnits();
            result.RentUnitResults = RentUnits;
            return Ok(RealEstateResponse<RentUnitResultDto>.Create(result));
        }
        [HttpGet]
        [AllowAnonymous]
        [Route(nameof(GetEstate_Types)), ResponseType(typeof(RealEstateResponse<Estate_TypeResultDto>))]
        public async Task<IHttpActionResult> GetEstate_Types()
        {
            Estate_TypeResultDto result = new Estate_TypeResultDto();
            List<RealEstateEstate_TypeDto> items = _service.GetAllEstate_Types();
            result.Estate_TypeResults = items;
            return Ok(RealEstateResponse<Estate_TypeResultDto>.Create(result));
        }
        [HttpGet]
        [AllowAnonymous]
        [Route(nameof(GetTowns)), ResponseType(typeof(RealEstateResponse<TownResultDto>))]
        public async Task<IHttpActionResult> GetTowns()
        {
            TownResultDto result = new TownResultDto();
            List<RealEstateTownDto> items = _service.GetAllTowns();
            result.TownResults = items;
            return Ok(RealEstateResponse<TownResultDto>.Create(result));
        }

        [HttpGet]
        [AllowAnonymous]
        [Route(nameof(GetMasterData)), ResponseType(typeof(RealEstateResponse<MasterDataDto>))]
        public async Task<IHttpActionResult> GetMasterData()
        {
            MasterDataDto result = new MasterDataDto { MasterData = new MasterDataResult() };
            result.MasterData.Towns = _service.GetAllTowns();
            result.MasterData.ListingTypes = _service.GetAllEstate_Types();
            result.MasterData.SaleUnits = _service.GetAllSaleUnits();
            result.MasterData.RentUnits = _service.GetAllRentUnits();
            return Ok(RealEstateResponse<MasterDataDto>.Create(result));
        }

        [HttpGet]
        [JwtAuthentication]
        [Route(nameof(GetLoginLogs)), ResponseType(typeof(RealEstateResponse<LoginLogResultDto>))]
        public async Task<IHttpActionResult> GetLoginLogs(int page = 1, int pageSize = 10)
        {
            LoginLogResultDto result = new LoginLogResultDto();
            List<RealEstateLoginLogDto> Estates = _service.GetAllLoginLogs(page, pageSize);
            result.LoginLogResults = Estates;
            return Ok(RealEstateResponse<LoginLogResultDto>.Create(result));
        }
        [HttpPost]
        [JwtAuthentication]
        [Route(nameof(CreateLoginLog)), ResponseType(typeof(RealEstateResponse<bool>))]
        public async Task<IHttpActionResult> CreateLoginLog(RealEstateLoginLogDto arguments)
        {
            bool update = _service.CreateLoginLog(arguments);

            return Ok(RealEstateResponse<bool>.Create(update));
        }
        #endregion
    }
}