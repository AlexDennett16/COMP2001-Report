using Microsoft.EntityFrameworkCore;

namespace COMP_2001_Report.Data
{
    public class LoginContext
    {
        public DbSet<AuthUser> AuthUsers { get; set; }
    }
}
