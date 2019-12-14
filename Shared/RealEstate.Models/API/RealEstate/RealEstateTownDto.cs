
namespace RealEstate.Models.API.RealEstate
{
    public class RealEstateTownDto
    {
        public long? TownId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public System.DateTime? CreateDate { get; set; }
        public bool? IsDelete { get; set; }
        public int TotalProduct { get; set; }

    }
}
