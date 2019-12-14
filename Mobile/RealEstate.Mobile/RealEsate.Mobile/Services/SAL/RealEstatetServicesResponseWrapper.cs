using System;
using System.Net;
using RealEstate.Models.API;
using RealEstate.Models.API;

namespace RealEstate.Mobile.Services.SAL
{
    public class RealEstateServicesResponseWrapper
    {
        public HttpStatusCode HttpCode { get; set; }
        public bool IsSuccess => ((int)HttpCode >= 200 && (int)HttpCode < 300) || HttpCode == default(int);
        public Exception Exception { get; set; }
    }

    public class RealEstatetServicesResponseWrapper<TData, TError> : RealEstateServicesResponseWrapper
    {
        public RealEstateResponseWithIssue<TData, TError> Response { get; set; }
    }
}
