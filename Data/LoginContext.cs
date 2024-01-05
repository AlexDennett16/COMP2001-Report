using COMP_2001_Report.Models;
using Microsoft.EntityFrameworkCore;

namespace COMP_2001_Report.Data
{
    public class LoginContext
    {
        public required DbSet<LoginRequestModel> AuthUsers { get; set; }
    }
}
