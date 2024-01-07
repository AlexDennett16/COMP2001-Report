namespace COMP_2001_Report.Models
{
    //Finding User by id and PUT
    public class Users
    {
        public int user_id { get; internal set; }
        public required string? username { get; set; }
        public required string? password { get; set; }
        public required string? email { get; set; }

    }

    //Used for mot other user interactions, POST, PUT
    public class UserModel
    {
        public required string? username { get; set; }
        public required string? password { get; set; }
        public required string? email { get; set; }
    }

    //Only used for unarchiving a user
    public class Archive_User
    {
        public required int archive_user_id { get; set; }
        public required string? username { get; set; }
        public required string? password { get; set; }
        public required string? email { get; set; }
    }

    //Used soley for handling login details
    public class LoginRequestModel
    {
        public string email { get; set; }
        public string password { get; set; }
    }

}

