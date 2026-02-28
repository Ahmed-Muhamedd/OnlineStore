using Microsoft.AspNetCore.Identity;

namespace OnlineStore.Data
{
    public class ApplicationUser:IdentityUser
    {
        public string FirstName { set; get; } = null!;
        public string SecondName { set; get; } = null!;
    }
}
