using System;

namespace COMP_2001_Report.Models
{
    public class Users
    {
        public int Id { get; internal set; }
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public required string Email { get; set; }
        
    }
}
