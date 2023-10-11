using System.Text.RegularExpressions;

namespace UsersAPI.Models.Business
{
    public class User
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public List<Role> Roles { get; set; } = new List<Role>();

        public User()
        {

        }
        public User(long id, string name, int age, string email)
        {
            Id = id;
            Name = name;
            Age = age;
            Email = email;
        }

        public User(long id, string name, int age, string email, List<Role> roles)
        {
            Id = id;
            Name = name;
            Age = age;
            Email = email;
            Roles = roles;
        }

        public bool IsCorrect()
        {
            Regex regex = new Regex(@"[A-z,0-9]+\@[A-z]{1,6}\.[A-z]{2,3}$");
            if (Id <= 0)
            {
                return false;
            }
            else if (Name is null || Name == "")
            {
                return false;
            }
            else if (Age <= 0)
            {
                return false;
            }
            else if (Email is null || !regex.IsMatch(Email))
            {
                return false;
            }
            else if (Roles is null || Roles.Count == 0 || !Roles.All(x => x.IsCorrect())
                || Roles.Any(x => Roles.Count(y => y.Name == x.Name) != 1))
            {
                return false;
            }
            return true;
        }
    }
}
