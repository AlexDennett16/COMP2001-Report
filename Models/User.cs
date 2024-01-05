namespace COMP_2001_Report.Models
{
    public class Users
    {
        public int user_id { get; internal set; }
        public required string? username { get; set; }
        public required string? password { get; set; }
        public required string? email { get; set; }

    }

    public class UserModel
    {
        public required string? username { get; set; }
        public required string? password { get; set; }
        public required string? email { get; set; }
    }

    public class Archive_User
    {
        public required int archive_user_id { get; set; }
        public required string? username { get; set; }
        public required string? password { get; set; }
        public required string? email { get; set; }
    }
    public class LoginRequestModel
    {
        public string email { get; set; }
        public string password { get; set; }
    }

}

