using Microsoft.AspNetCore.Identity;

namespace Lab_Three_.Data.Models
{
    public class Employee : IdentityUser
    {
      
        public string Department { get; set; } = string.Empty;
    }
}
