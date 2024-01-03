using System;

namespace COMP_2001_Report
{
    public class Users
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public required string Email { get; set; }
        public int Id { get; internal set; }
    }
}
