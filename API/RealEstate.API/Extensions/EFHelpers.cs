using RealEstate.API.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Web;

namespace RealEstate.API.Extensions
{
    public static class EFHelpers
    {
        public static bool IsEqual(this string source, string param)
        {
            if (string.IsNullOrEmpty(source))
                return false;
            return (Regex.Replace(source, @"[^\w\d]", "")).Equals((Regex.Replace(param, @"[^\w\d]", "")), StringComparison.CurrentCultureIgnoreCase);
        }
    }
}