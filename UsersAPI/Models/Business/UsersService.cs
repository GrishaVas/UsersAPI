using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text.RegularExpressions;
using UsersAPI.Models.DatabaseContext;

namespace UsersAPI.Models.Business
{
    public class UsersService
    {
        private UsersDataBaseContext _dbcontext;

        public UsersService(UsersDataBaseContext context)
        {
            _dbcontext = context;
        }

        public List<User> GetUsers(int? offset, int? limit)
        {
            if (offset == 0 && limit == 0)
            {
                return _dbcontext.Users.Include(x => x.Roles).ToList();
            }
            else if (offset + limit <= _dbcontext.Users.Count() && offset >= 0 && limit >= 0)
            {
                return _dbcontext.Users.FromSqlRaw($"select * from users limit {offset}, {limit}").Include(x => x.Roles).ToList();
            }
            else return null;
        }

        public User GetUser(long id)
        {
            return _dbcontext.Users.Include(x => x.Roles).ToList().Find(x => x.Id == id);
        }

        public List<User> GetSortedUsersByProp(string propName, int order)
        {
            PropertyInfo propertyInfo = typeof(User).GetProperty(propName);
            List<User> users = _dbcontext.Users.Include(x => x.Roles).ToList();
            if (propertyInfo is not null)
            {
                if (propName == "Roles")
                {
                    users = users.OrderBy(x => x.Roles.OrderBy(x => x.Name).First().Name).ToList();
                }
                else users = users.OrderBy(x => x.Name).ToList();
                if (order == 1)
                {
                    return users;
                }
                else return users.Reverse<User>().ToList();
            }
            else return null; ;
        }

        public List<User> GetFilteredUsers(string propName, string value)
        {
            PropertyInfo propertyInfo = typeof(User).GetProperty(propName);
            List<User> users = _dbcontext.Users.Include(x => x.Roles).ToList();
            if (propertyInfo is not null)
            {
                if (propName == "Roles")
                {
                    return users.Where(x => x.Roles.Any(x => x.Name == value)).ToList();
                }
                return users.Where(x => typeof(User).GetProperty(propName).GetValue(x).ToString() == value).ToList();
            }
            else return null;
        }

        public string AddUser(User user)
        {
            if (user is not null && user.IsCorrect())
            {
                if (!_dbcontext.Users.Any(x => x.Id == user.Id) && !_dbcontext.Users.Any(x => x.Email == user.Email))
                {
                    _dbcontext.Users.Add(user);
                    _dbcontext.SaveChanges();
                    return "User has added";
                }
                else return "Uncorrect Id or Email!";
            }
            else return "User is uncorrect!";
        }

        public string DeleteUser(long id)
        {
            User user = _dbcontext.Users.Find(id);
            if (user is not null)
            {
                _dbcontext.Users.Remove(user);
                _dbcontext.SaveChanges();
                return "User has deleted!";
            }
            else return $"User with id:{id} does not exist!";
        }

        public string ChangeUser(long id, User newUser)
        {
            User user = _dbcontext.Users.Find(id);
            _dbcontext.Entry(user).Collection(x => x.Roles).Load();
            if (user is not null)
            {
                if (newUser.IsCorrect())
                {
                    if (_dbcontext.Users.Any(x => x.Email == newUser.Email))
                    {
                        return "The Email is already exists.";
                    }
                    user.Id = newUser.Id;
                    user.Name = newUser.Name;
                    user.Roles = newUser.Roles;
                    user.Age = newUser.Age;
                    user.Email = newUser.Email;
                    _dbcontext.SaveChanges();
                    return "User has changed!";
                }
                else return "User is uncorrect!";
            }
            else return $"User with id:{id} does not exist!";
        }

        public string AddRoleToUser(long id, Role role)
        {
            User user = _dbcontext.Users.Find(id);
            if (user is not null)
            {
                if (role.IsCorrect())
                {
                    user.Roles.Add(role);
                    _dbcontext.SaveChanges();
                    return "Role has added!";
                }
                else return "Role is uncorrect"; ;
            }
            else return $"User with id:{id} does not exist!";
        }

    }
}
