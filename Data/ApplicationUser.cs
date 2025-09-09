using Microsoft.AspNetCore.Identity;
namespace PunchList.Data;
public class ApplicationUser : IdentityUser
{
    public string? DisplayName { get; set; }
}
