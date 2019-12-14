//using FirebaseNet.Messaging;
using RealEstate.API.Common;
using RealEstate.API.DAL;
using RealEstate.API.Extensions;
using RealEstate.API.Notifications;
using RealEstate.API.SAL.RealEstateService;
using RealEstate.Models.API.RealEstate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;

namespace RealEstate.API.SAL.RealEstateService
{
    public class RealEstateService : BaseService, IRealEstateService
    {
        RealEstateEntities _realEstateEntities;
        private const int endDate = 90;
        public RealEstateService(RealEstateEntities realEstateEntities) : base(realEstateEntities)
        {
            _realEstateEntities = realEstateEntities;
        }

        public RealEstateDashboardDto GetDashBoard()
        {
            var model = new RealEstateDashboardDto
            {
                AccountsTotal = _realEstateEntities.Accounts.Where(x => x.IsDelete == false).Count(),
                EstateTotal = _realEstateEntities.Estates.Where(x => x.IsDelete == false).Count(),
                HotEstateTotal = _realEstateEntities.Estates.Where(x => x.IsDelete == false && x.IsHot == true).Count(),
                NotificationTotal = _realEstateEntities.Notifications.Where(x => x.IsDelete == false).Count()
            };
            return model;
        }
        #region Account      
        /// <summary>
        ///  Account 
        /// </summary>
        /// <returns></returns>
        public List<RealEstateAccountDto> GetAllAccounts()
        {

            var model = _realEstateEntities.Accounts.Select(x => new RealEstateAccountDto
            {
                FirstName = x.FirstName,
                LastName = x.LastName,
                UserName = x.UserName,
                AccountId = x.AccountId,
                CreateDate = x.CreateDate,
                Department = new RealEstateDepartmentDto
                {
                    DepartmentId = x.DepartmentId,
                    DepartmentName = x.Department.DepartmentName,
                },
            }).ToList();
            return model;
        }
        public bool UpdateAccount(RealEstateAccountDto model)
        {
            try
            {
                var item = _realEstateEntities.Accounts.Find(model.AccountId);
                item.Email = model.Email;
                item.FirstName = model.FirstName;
                item.LastName = model.LastName;
                item.UserName = model.UserName;
                item.DepartmentId = model.Department.DepartmentId;
                _realEstateEntities.SaveChanges();
                return true;
            }
            catch (System.Exception)
            {

                return false;
            }
        }
        public bool ChangePassword(RealEstateAccountChangePasswordDto model)
        {
            try
            {
                var item = _realEstateEntities.Accounts.Find(model.AccountId);
                if (model.NewPassword != model.ConfirmNewPassword)
                {
                    return false;
                }
                if (model.CurrentPassword != Security.MD5Decrypt(item.Password))
                {
                    return false;
                }
                item.Password = Security.MD5Encrypt(model.NewPassword);
                _realEstateEntities.SaveChanges();
                return true;
            }
            catch (System.Exception)
            {

                return false;
            }
        }
        public bool CreateAccount(RealEstateAccountDto model)
        {
            try
            {
                var now = DateTime.Now;
                var item = new Account
                {
                    FirstName = model.FirstName,
                    CreateDate = now,
                    Email = model.Email,
                    LastName = model.LastName,
                    DepartmentId = model.Department.DepartmentId,
                    IsDelete = false,
                };

                _realEstateEntities.Accounts.Add(item);
                return true;
            }
            catch (System.Exception)
            {

                return false;
            }
        }


        public LoginModel GetLoginByEmail(string Email)
        {
            LoginModel lg = new LoginModel();
            var query = (from src in _realEstateEntities.Accounts
                         where src.Email == Email && src.IsDelete == false
                         select new LoginModel
                         {
                             AccountId = src.AccountId,
                             Email = src.Email,
                             Password = src.Password,
                             FirstName = src.FirstName,
                             LastName = src.LastName
                         }
                             ).FirstOrDefault();

            if (query != null)
                lg = query;
            return lg;
        }

        public List<string> GetRoleNameByAccountId(long accountId)
        {
            //_data.AccountRoles.Join().Where(x=> x.AccountId == accountId).Select(x=> x.ro)
            var Ar = _realEstateEntities.AccountRoles.Where(x => x.AccountId == accountId && x.IsDelete == false).ToList();
            List<string> kq = new List<string>();
            if (Ar != null)
            {
                foreach (var item in Ar)
                {
                    kq.Add(item.Role.RoleName);
                }
            }
            return kq;
        }

        #endregion

        #region Department       
        public List<RealEstateDepartmentDto> GetAllDepartments()
        {
            var model = _realEstateEntities.Departments.Where(x => x.IsDelete == false).Select(x => new RealEstateDepartmentDto
            {
                DepartmentId = x.DepartmentId,
                CreatDate = x.CreatDate,
                DepartmentName = x.DepartmentName,
                IsDelete = x.IsDelete
            }).ToList();
            return model;
        }
        #endregion

        #region Estate
        public EstateResultDto GetAllEstates(ProductArguments productArguments)
        {
            try
            {
                var dayAgo = DateTime.Now.AddDays(-90);

                var skip = (productArguments.PageIndex - 1) * productArguments.PageLimit;
                var expression = PredicateBuilder.True<Estate>();

                expression = PredicateBuilder.And(expression, x => dayAgo <= x.CreatedDate);
                expression = PredicateBuilder.And(expression, x => x.IsDelete != null && x.IsDelete == false);

                if (!string.IsNullOrEmpty(productArguments.ProductCode))
                    expression = PredicateBuilder.And(expression, x => ((x.EstateCode ?? "").Replace(".", "").Replace("-", "").Replace("_", "").Replace(" ", "").Contains(productArguments.ProductCode.Replace(".", "").Replace("-", "").Replace("_", "").Replace(" ", ""))));
                if (productArguments.TownId.HasValue)
                    expression = PredicateBuilder.And(expression, x => x.TownId == productArguments.TownId.Value);
                if (productArguments.ListingTypeId.HasValue)
                    expression = PredicateBuilder.And(expression, x => x.Estates_TypeId == productArguments.ListingTypeId.Value);
                //if(productArguments.Area != null)
                //    expression = PredicateBuilder.And(expression, x => x.Area <= productArguments.Area.To && x.Area >= productArguments.Area.From);

                if (productArguments.Type == 1 && productArguments.PriceRange != null)
                    expression = PredicateBuilder.And(expression, x => x.RentPrice <= productArguments.PriceRange.To && x.RentPrice >= productArguments.PriceRange.From);
                else if (productArguments.Type == 0 && productArguments.PriceRange != null)
                    expression = PredicateBuilder.And(expression, x => x.SalePrice <= productArguments.PriceRange.To && x.SalePrice >= productArguments.PriceRange.From);

                if (productArguments.AccountId.HasValue)
                    expression = PredicateBuilder.And(expression, x => x.AccountId == productArguments.AccountId.Value);
                var products = _realEstateEntities.Estates.Where(expression);
                var totalRecords = _realEstateEntities.Estates.Count(expression);
                IOrderedQueryable<Estate> orderQuery;
                switch (productArguments.SortingType)
                {
                    case SortingType.RentPriceDescending:
                        orderQuery = products.OrderByDescending(x => x.RentPrice);
                        break;
                    case SortingType.RentPriceAscending:
                        orderQuery = products.OrderBy(x => x.RentPrice);
                        break;
                    case SortingType.SalePriceDescending:
                        orderQuery = products.OrderByDescending(x => x.SalePrice);
                        break;
                    case SortingType.SalePriceAscending:
                        orderQuery = products.OrderBy(x => x.SalePrice);
                        break;
                    case SortingType.AreaDescending:
                        orderQuery = products.OrderByDescending(x => x.Area);
                        break;
                    case SortingType.AreaAscending:
                        orderQuery = products.OrderBy(x => x.Area);
                        break;
                    case SortingType.ProductCodeDescending:
                        orderQuery = products.OrderByDescending(x => x.EstateCode);
                        break;
                    case SortingType.ProductCodeAscending:
                        orderQuery = products.OrderBy(x => x.EstateCode);
                        break;
                    case SortingType.Oldest:
                        orderQuery = products.OrderBy(x => x.ModifiedDate);
                        break;
                    default:
                        orderQuery = products.OrderByDescending(x => x.ModifiedDate); //lasted
                        break;
                }
                var model = orderQuery.Select(x => new RealEstateEstateDto
                {
                    Account = new RealEstateAccountDto
                    {
                        AccountId = x.AccountId,
                        FirstName = x.Account.FirstName,
                        LastName = x.Account.LastName,
                        Email = x.Account.Email,
                        Mobile = x.Account.Mobile,
                        UserName = x.Account.UserName,
                    },
                    SaleOrRent = x.SaleOrRent,
                    Area = x.Area,
                    SaleUnit = new RealEstateSaleUnitDto
                    {
                        SaleUnitId = x.SaleUnitId,
                        Name = x.SaleUnit.Name
                    }
                     ,
                    RentUnit = new RealEstateRentUnitDto
                    {
                        RentUnitId = x.RentUnitId,
                        Name = x.RentUnit.Name
                    },
                    Note = x.Note,
                    SalePrice = x.SalePrice,
                    RentPrice = x.RentPrice,
                    Id = x.Id,
                    IsHot = x.IsHot,
                    Town = new RealEstateTownDto
                    {
                        TownId = x.TownId,
                        Address = x.Town.Address,
                        Name = x.Town.Name,
                    },
                    EstateCode = x.EstateCode,
                    Estate_TypeId = x.Estates_TypeId,
                    NameEstate_Type = x.Estate_Types.Name,
                    LotCode = x.LotCode,
                    CreatedDate = x.CreatedDate,
                    ModifiedDate = x.ModifiedDate,
                    Phone = x.Phone,
                    HouseNumber = x.HouseNumber,
                    OwnerName = x.OwnerName,
                    Lat = x.Lat,
                    Long = x.Long,
                    MainPinText = x.PinMainText,
                    Address = x.Address,
                    ImageUrls = x.EstateMedias.Select(y => new EstateImageDto
                    {
                        Id = y.Id,
                        Name = y.FriendlyFileName,
                        Url = y.Url
                    }).ToList(),
                    IsDelete = x.IsDelete
                }).Skip(skip).Take(productArguments.PageLimit).ToList();
                foreach(var item in model)
                {
                    if (item.ImageUrls != null && item.ImageUrls.Any())
                    {
                        foreach(var imageUrl in item.ImageUrls)
                        {
                            imageUrl.Url = $"{GetBaseUrl()}{imageUrl.Url}";
                        }
                    }
                }
                return new EstateResultDto
                {
                    EstateResults = model,
                    TotalRecord = totalRecords
                };
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public static string GetBaseUrl()
        {
            var request = HttpContext.Current.Request;
            var appRootFolder = request.ApplicationPath;
            if (!appRootFolder.EndsWith("/"))
            {
                appRootFolder += "/";
            }
            return string.Format(
                "{0}://{1}{2}",
                request.Url.Scheme,
                request.Url.Authority,
                appRootFolder
            );
        }
        public EstateResultDto GetAllMyEstates(ProductArguments productArguments)
        {
            try
            {
                if (productArguments.AccountId == null)
                {
                    return null;
                }
                var skip = (productArguments.PageIndex - 1) * productArguments.PageLimit;
                var expression = PredicateBuilder.True<Estate>();
                switch (productArguments.ProductType)
                {
                    case ProductType.Expired:
                        var dayAgo = DateTime.Now.AddDays(-90);
                        expression = PredicateBuilder.And(expression, x => dayAgo > x.ModifiedDate);
                        break;
                    case ProductType.Deleted:
                        expression = PredicateBuilder.And(expression, x => x.IsDelete != null && x.IsDelete == true);
                        break;
                    case ProductType.WillBeExpired:
                        var dayAgo60 = DateTime.Now.AddDays(-60);
                        var dayAgo90 = DateTime.Now.AddDays(-90);
                        expression = PredicateBuilder.And(expression, x => dayAgo90 <= x.ModifiedDate && x.ModifiedDate <= dayAgo60);
                        break;
                    default:
                        var day = DateTime.Now.AddDays(-90);
                        expression = PredicateBuilder.And(expression, x => day <= x.ModifiedDate && x.IsDelete == false);
                        break;
                }
                if (!string.IsNullOrEmpty(productArguments.ProductCode))
                    expression = PredicateBuilder.And(expression, x => ((x.EstateCode ?? "").Replace(".", "").Replace("-", "").Replace("_", "").Replace(" ", "").Contains(productArguments.ProductCode.Replace(".", "").Replace("-", "").Replace("_", "").Replace(" ", ""))));
                if (productArguments.TownId.HasValue)
                    expression = PredicateBuilder.And(expression, x => x.TownId == productArguments.TownId.Value);
                if (productArguments.ListingTypeId.HasValue)
                    expression = PredicateBuilder.And(expression, x => x.Estates_TypeId == productArguments.ListingTypeId.Value);

                //if(productArguments.Area != null)
                //    expression = PredicateBuilder.And(expression, x => x.Area <= productArguments.Area.To && x.Area >= productArguments.Area.From);

                if (productArguments.Type == 1 && productArguments.PriceRange != null)
                    expression = PredicateBuilder.And(expression, x => x.RentPrice <= productArguments.PriceRange.To && x.RentPrice >= productArguments.PriceRange.From);
                else if (productArguments.Type == 0 && productArguments.PriceRange != null)
                    expression = PredicateBuilder.And(expression, x => x.SalePrice <= productArguments.PriceRange.To && x.SalePrice >= productArguments.PriceRange.From);


                if (productArguments.AccountId.HasValue)
                    expression = PredicateBuilder.And(expression, x => x.AccountId == productArguments.AccountId.Value);

                var products = _realEstateEntities.Estates.Where(expression);
                var totalRecords = _realEstateEntities.Estates.Count(expression);
                IOrderedQueryable<Estate> orderQuery;
                switch (productArguments.SortingType)
                {
                    case SortingType.RentPriceDescending:
                        orderQuery = products.OrderByDescending(x => x.RentPrice);
                        break;
                    case SortingType.RentPriceAscending:
                        orderQuery = products.OrderBy(x => x.RentPrice);
                        break;
                    case SortingType.SalePriceDescending:
                        orderQuery = products.OrderByDescending(x => x.SalePrice);
                        break;
                    case SortingType.SalePriceAscending:
                        orderQuery = products.OrderBy(x => x.SalePrice);
                        break;
                    case SortingType.AreaDescending:
                        orderQuery = products.OrderByDescending(x => x.Area);
                        break;
                    case SortingType.AreaAscending:
                        orderQuery = products.OrderBy(x => x.Area);
                        break;
                    case SortingType.ProductCodeDescending:
                        orderQuery = products.OrderByDescending(x => x.EstateCode);
                        break;
                    case SortingType.ProductCodeAscending:
                        orderQuery = products.OrderBy(x => x.EstateCode);
                        break;
                    case SortingType.Oldest:
                        orderQuery = products.OrderBy(x => x.ModifiedDate);
                        break;
                    default:
                        orderQuery = products.OrderByDescending(x => x.ModifiedDate); //lasted
                        break;
                }
                var model = orderQuery.Select(x => new RealEstateEstateDto
                {
                    Account = new RealEstateAccountDto
                    {
                        AccountId = x.AccountId,
                        FirstName = x.Account.FirstName,
                        LastName = x.Account.LastName,
                        Email = x.Account.Email,
                        Mobile = x.Account.Mobile,
                        UserName = x.Account.UserName,
                    },
                    SaleOrRent = x.SaleOrRent,
                    Area = x.Area,
                    SaleUnit = new RealEstateSaleUnitDto
                    {
                        SaleUnitId = x.SaleUnitId,
                        Name = x.SaleUnit.Name
                    }
                     ,
                    RentUnit = new RealEstateRentUnitDto
                    {
                        RentUnitId = x.RentUnitId,
                        Name = x.RentUnit.Name
                    },
                    Note = x.Note,
                    SalePrice = x.SalePrice,
                    RentPrice = x.RentPrice,
                    Id = x.Id,
                    IsHot = x.IsHot,
                    Town = new RealEstateTownDto
                    {
                        TownId = x.TownId,
                        Address = x.Town.Address,
                        Name = x.Town.Name,
                    },
                    EstateCode = x.EstateCode,
                    Estate_TypeId = x.Estates_TypeId,
                    NameEstate_Type = x.Estate_Types.Name,
                    LotCode = x.LotCode,
                    CreatedDate = x.CreatedDate,
                    ModifiedDate = x.ModifiedDate,
                    Phone = x.Phone,
                    HouseNumber = x.HouseNumber,
                    OwnerName = x.OwnerName,
                    Lat = x.Lat,
                    Long = x.Long,
                    MainPinText = x.PinMainText,
                    Address = x.Address,
                    ImageUrls = x.EstateMedias.Select(y => new EstateImageDto
                    {
                        Id = y.Id,
                        Name = y.FriendlyFileName,
                        Url = y.Url
                    }).ToList(),
                    IsDelete = x.IsDelete
                }).Skip(skip).Take(productArguments.PageLimit).ToList();
                foreach (var item in model)
                {
                    if (item.ImageUrls != null && item.ImageUrls.Any())
                    {
                        foreach (var imageUrl in item.ImageUrls)
                        {
                            imageUrl.Url = $"{GetBaseUrl()}{imageUrl.Url}";
                        }
                    }
                }
                return new EstateResultDto
                {
                    EstateResults = model,
                    TotalRecord = totalRecords
                };
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public bool UpdateEstate(RealEstateEstateDto model)
        {
            try
            {
                var now = DateTime.Now;
                var item = _realEstateEntities.Estates.Find(model.Id);
                if (model.Account.AccountId != item.AccountId || item.IsDelete == true)
                {
                    return false;
                }
                item.TownId = model.Town.TownId;
                item.EstateCode = model.EstateCode;
                item.CreatedDate = now;
                item.LotCode = model.LotCode;
                item.HouseNumber = model.HouseNumber;
                item.OwnerName = model.OwnerName;
                item.SalePrice = model.SalePrice;
                item.RentPrice = model.RentPrice;
                item.Note = model.Note;
                item.IsHot = model.IsHot;
                item.AccountId = model.Account.AccountId;
                item.SaleUnitId = model.SaleUnit.SaleUnitId;
                item.RentUnitId = model.RentUnit.RentUnitId;
                item.Area = model.Area;
                item.Phone = model.Phone;
                item.IsDelete = false;
                item.Estates_TypeId = model.Estate_TypeId;
                bool modelChang = false;


                if (item.EstateCode != model.EstateCode)
                {
                    if (modelChang)
                    {
                        model.LogBy = model.LogBy + "Edit Estate Code:" + model.EstateCode.ToString() + " From " + item.EstateCode.ToString() + " at " + DateTime.Now.ToString() + "</br>";
                    }
                    else
                    {
                        model.LogBy = item.LogBy + "Edit Estate Code:" + model.EstateCode.ToString() + " From " + item.EstateCode.ToString() + " at " + DateTime.Now.ToString() + "</br>";
                    }
                    modelChang = true;
                }
                if (item.SalePrice != model.SalePrice)
                {
                    if (modelChang)
                    {
                        model.LogBy = model.LogBy + "Edit Sale Price:" + model.SalePrice.ToString() + " From " + item.SalePrice.ToString() + " at " + DateTime.Now.ToString() + "</br>";
                    }
                    else
                    {
                        model.LogBy = item.LogBy + "Edit Sale Price:" + model.SalePrice.ToString() + " From " + item.SalePrice.ToString() + " at " + DateTime.Now.ToString() + "</br>";
                    }
                    modelChang = true;

                }

                if (item.RentPrice != model.RentPrice)
                {
                    if (modelChang)
                    {
                        model.LogBy = model.LogBy + "Edit Rent Price: " + model.RentPrice.ToString() + " From " + item.RentPrice.ToString() + " at " + DateTime.Now.ToString() + "</br>";
                    }
                    else
                    {
                        model.LogBy = item.LogBy + "Edit Rent Price: " + model.RentPrice.ToString() + " From " + item.RentPrice.ToString() + " at " + DateTime.Now.ToString() + "</br>";
                    }
                    modelChang = true;
                }
                if (item.OwnerName != model.OwnerName)
                {
                    if (modelChang)
                    {
                        model.LogBy = model.LogBy + "Edit Owner Name: " + model.OwnerName.ToString() + " From " + item.OwnerName.ToString() + " at " + DateTime.Now.ToString() + "</br>";
                    }
                    else
                    {
                        model.LogBy = item.LogBy + "Edit Owner Name: " + model.OwnerName.ToString() + " From " + item.OwnerName.ToString() + " at " + DateTime.Now.ToString() + "</br>";
                    }
                    modelChang = true;
                }
                if (item.Phone != model.Phone)
                {
                    if (modelChang)
                    {
                        model.LogBy = model.LogBy + "Edit Phone: " + model.Phone.ToString() + " From " + item.Phone.ToString() + " at " + DateTime.Now.ToString() + "</br>";
                    }
                    else
                    {
                        model.LogBy = item.LogBy + "Edit Phone: " + model.Phone.ToString() + " From " + item.Phone.ToString() + " at " + DateTime.Now.ToString() + "</br>";
                    }

                    modelChang = true;
                }

                if (item.EstateCode != model.EstateCode)
                {
                    if (modelChang)
                    {
                        model.LogBy = model.LogBy + "Edit EstateCode: " + model.EstateCode.ToString() + " From " + item.EstateCode.ToString() + " at " + DateTime.Now.ToString() + "</br>";
                    }
                    else
                    {
                        model.LogBy = item.LogBy + "Edit EstateCode: " + model.EstateCode.ToString() + " From " + item.EstateCode.ToString() + " at " + DateTime.Now.ToString() + "</br>";
                    }

                    modelChang = true;
                }
                if (item.LotCode != model.LotCode)
                {
                    if (modelChang)
                    {
                        model.LogBy = model.LogBy + "Edit Town Id: " + model.LotCode.ToString() + " From " + item.LotCode.ToString() + " at " + DateTime.Now.ToString() + "</br>";
                    }
                    else
                    {
                        model.LogBy = item.LogBy + "Edit Town Id: " + model.LotCode.ToString() + " From " + item.LotCode.ToString() + " at " + DateTime.Now.ToString() + "</br>";
                    }
                    modelChang = true;

                }
                _realEstateEntities.SaveChanges();
                var Town = item.Town.Name;
                var EstateCode = item.EstateCode;
                Task.Run(async () =>
                {
                    await NotificationSender.Instance.Send($"Notification ", $" {EstateCode} - {Town} has edited");
                });
                return true;

            }
            catch (System.Exception ex)
            {

                return false;
            }
        }
        public bool UpdateDeleteEstate(RealEstateDeleteEstateDto model)
        {
            try
            {
                var now = DateTime.Now;

                var item = _realEstateEntities.Estates.Find(model.Id);
                if (model.AccountId != item.AccountId)
                {
                    return false;
                }
                item.IsDelete = model.IsDelete;
                item.CreatedDate = now;
                _realEstateEntities.SaveChanges();
                var Town = item.Town.Name;
                var EstateCode = item.EstateCode;
                Task.Run(() =>
                {
                    NotificationSender.Instance.Send($"Notification ", $" {EstateCode} - {Town} was deleted");
                });
                return true;
            }
            catch (System.Exception)
            {

                return false;
            }
        }
        public bool CreateEstate(RealEstateEstateDto model)
        {
            try
            {
                var now = DateTime.Now;
                var item = new Estate
                {
                    TownId = model.Town.TownId,
                    EstateCode = model.EstateCode,
                    CreatedDate = now,
                    ModifiedDate = now,
                    LotCode = model.LotCode,
                    HouseNumber = model.HouseNumber,
                    OwnerName = model.OwnerName,
                    SalePrice = model.SalePrice,
                    RentPrice = model.RentPrice,
                    Note = model.Note,
                    IsHot = model.IsHot,
                    AccountId = model.Account.AccountId,
                    SaleUnitId = model.SaleUnit.SaleUnitId,
                    RentUnitId = model.RentUnit.RentUnitId,
                    Area = model.Area,
                    Phone = model.Phone,
                    Lat = model.Lat,
                    Long = model.Long,
                    PinMainText = model.MainPinText,
                    Address = model.Address,
                    Estates_TypeId = model.Estate_TypeId,
                    IsDelete = false,
                };

                _realEstateEntities.Estates.Add(item);
                _realEstateEntities.SaveChanges();
                if (model.EstateImageIds.Any())
                {
                    foreach(var imageId in model.EstateImageIds)
                    {
                        var estateImage = _realEstateEntities.EstateMedias.FirstOrDefault(x => x.Id == imageId);
                        if(estateImage != null)
                        {
                            estateImage.EstateId = item.Id;
                        }
                    }
                    _realEstateEntities.SaveChanges();
                }
                var EstateCode = item.EstateCode;
                Task.Run(async () =>
                {
                    await NotificationSender.Instance.Send($"Notification ", $" {EstateCode} was created");
                });
                return true;

            }
            catch (System.Exception ex)
            {

                return false;
            }
        }
        #endregion

        #region Town
        public List<RealEstateTownDto> GetAllTowns()
        {
            var ninetyDayAgo = DateTime.Now.AddDays(-90);
            var model = _realEstateEntities.Towns.Where(x => x.IsDelete == false).OrderBy(x => x.Name).Select(x => new RealEstateTownDto
            {
                Address = x.Address,
                CreateDate = x.CreateDate,
                TownId = x.TownId,
                Name = x.Name,
                TotalProduct = x.Estates.Count(y => y.IsDelete == false && y.CreatedDate >= ninetyDayAgo),
                IsDelete = x.IsDelete
            }).ToList();
            return model;
        }

        #endregion

        #region SaleUnit
        public List<RealEstateSaleUnitDto> GetAllSaleUnits()
        {

            var model = _realEstateEntities.SaleUnits.Select(x => new RealEstateSaleUnitDto
            {
                SaleUnitId = x.SaleUnitId,
                Name = x.Name,

            }).ToList();
            return model;
        }

        #endregion

        #region RentUnit
        public List<RealEstateRentUnitDto> GetAllRentUnits()
        {

            var model = _realEstateEntities.RentUnits.Select(x => new RealEstateRentUnitDto
            {
                RentUnitId = x.RentUnitId,
                Name = x.Name,
            }).ToList();
            return model;
        }

        #endregion

        #region RentUnit
        public List<RealEstateEstate_TypeDto> GetAllEstate_Types()
        {
            var ninetyDayAgos = DateTime.Now.AddDays(-90);
            var model = _realEstateEntities.Estate_Types.Where(x => x.IsDelete == false).Select(x => new RealEstateEstate_TypeDto
            {
                Estate_TypeId = x.Estate_TypeId,
                CreateDate = x.CreateDate,
                EditDate = x.EditDate,
                IsDelete = x.IsDelete,
                Name = x.Name,
                TotalProduct = x.Estates.Count(y => y.IsDelete == false && y.CreatedDate >= ninetyDayAgos)
            }).ToList();
            return model;
        }
        public List<RealEstateLoginLogDto> GetAllLoginLogs(int page, int pageSize)
        {
            var skip = (page - 1) * pageSize;

            var model = _realEstateEntities.LoginLogs.Select(x => new RealEstateLoginLogDto
            {
                Account = new RealEstateAccountDto
                {
                    AccountId = x.AccountId,
                    FirstName = x.Account.FirstName,
                    LastName = x.Account.LastName,
                    Email = x.Account.Email,
                    UserName = x.Account.UserName,
                },
                CreateDate = x.CreateDate,
                IPAddress = x.IPAddress,
                LoginLogId = x.LoginLogId,
                IsDelete = x.IsDelete
            }).Skip(skip).Take(pageSize).ToList();
            return model;
        }
        public bool CreateLoginLog(RealEstateLoginLogDto model)
        {
            try
            {
                var now = DateTime.Now;
                var item = new LoginLog
                {
                    AccountId = model.Account.AccountId,
                    CreateDate = now,
                    IPAddress = model.IPAddress,
                    IsDelete = false,
                };

                _realEstateEntities.LoginLogs.Add(item);
                return true;
            }
            catch (System.Exception)
            {

                return false;
            }
        }

        public async Task<long?> UploadFile(HttpPostedFile file)
        {
            if (file != null && file.ContentLength > 0)
            {
                string fileOnDisk = $"{Guid.NewGuid()}{System.IO.Path.GetExtension(file.FileName)}";
                string path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Uploads/Images"), fileOnDisk);
                file.SaveAs(path);
                var estateMedia = new EstateMedia
                {
                    FriendlyFileName = Path.GetFileName(file.FileName),
                    FileName = fileOnDisk,
                    Url = $"Uploads/Images/{fileOnDisk}", //TODO: Generate thumbnail image
                    IsDelete = false,
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow
                };
                _realEstateEntities.EstateMedias.Add(estateMedia);
                await _realEstateEntities.SaveChangesAsync();
                return estateMedia.Id;
            }
            return default(long?);
        }
        #endregion


    }
}