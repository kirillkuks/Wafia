using System.Net.Mail;

namespace WAFIA.Database.Types
{
    public class Account
    {
        public string Login { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        public long Id { get; set; }

        public Account(long id, string login, string mail, string password, bool isAdmin)
        {
            Id = id;
            Login = login;
            Mail = mail;
            Password = password;
            IsAdmin = isAdmin;
        }
    }
}
