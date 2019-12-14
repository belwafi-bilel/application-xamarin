
namespace RealEstate.Models.API.RealEstate
{
    public class RealEstateAccountDto
    {
        public long? AccountId { get; set; }
        public RealEstateDepartmentDto  Department { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public System.DateTime? LastSignIn { get; set; }
        public System.DateTime? CreateDate { get; set; }
        public string Image { get; set; }
        public string Detail { get; set; }
        public string Mobile { get; set; }
        public bool? IsDelete { get; set; }

    }
    public class RealEstateAccountChangePasswordDto
    {
        public long? AccountId { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }
    public class LoginModel
    {
        public long AccountId { get; set; }
     
        public string Email { get; set; }
      
        public string FirstName { get; set; }
      
        public string LastName { get; set; }
       
        public string Password { get; set; }
      
    }
}
