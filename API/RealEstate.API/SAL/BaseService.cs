using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RealEstate.API.DAL;

namespace RealEstate.API.SAL
{
    public class BaseService
    {
        public BaseService(RealEstateEntities db)
        {
            Db = db;
        }

        protected RealEstateEntities Db;
    }
}