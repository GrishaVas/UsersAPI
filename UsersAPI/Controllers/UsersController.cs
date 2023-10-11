using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using UsersAPI.Models;
using UsersAPI.Models.Business;

namespace UsersAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UsersService _usersService;

        public UsersController(UsersService usersService)
        {
            this._usersService = usersService;
        }

        /// <summary>
        /// Get Users with offset and limit or without params.
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <remarks>If "offset" and "limit" equals 0 returns all users, else returns range with offset and limit.</remarks>
        [HttpGet("{offset?}/{limit?}")]
        public ActionResult<List<User>> Get(int offset = 0, int limit = 0)
        {
            List<User> users = _usersService.GetUsers(offset, limit);
            if (users is not null)
            {
                return users;
            }
            else return NotFound();
        }

        /// <summary>
        /// Get one User.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>Returns one user by "id".</remarks>
        [HttpGet("/{id}")]
        public ActionResult<User> Get(int id)
        {
            User user = _usersService.GetUser(id);
            if (user is not null)
            {
                return user;
            }
            else return NotFound();
        }

        /// <summary>
        /// Sorted Users.
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        /// <remarks>Returns sorted users by property:"prop", order = 1 - sort ascending,else descending.</remarks>
        [HttpGet("SortedBy/{prop}/{order?}")]
        public ActionResult<List<User>> Sort(string prop, int order = 1)
        {
            List<User> users = _usersService.GetSortedUsersByProp(prop, order);
            if (users is not null)
            {
                return users;
            }else return NotFound();
        }

        /// <summary>
        /// Filter Users.
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <remarks>Returns filtered users by property:"prop" and "value".</remarks>
        [HttpGet("FilterBy/{prop}/{value}")]
        public ActionResult<List<User>> Filter(string prop,[FromRoute] string value)
        {
            List<User> users = _usersService.GetFilteredUsers(prop, value);
            if (users is not null)
            {
                return users;
            }
            else return NotFound();
        }

        /// <summary>
        /// Add User.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        /// <remarks>Adds user if users with "id" and "email" don`t exist and all properties are correct.</remarks>
        [Authorize]
        [HttpPost]
        public ActionResult<string> Post([FromBody]User user)
        {
            return _usersService.AddUser(user);
        }

        /// <summary>
        /// Add Role to User by id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        /// <remarks>Adds role to user if user with "id" exists and roles name is correct.</remarks>
        [Authorize]
        [HttpPost("AddRole")]
        public ActionResult<string> AddRole(long id, Role role)
        {
            return _usersService.AddRoleToUser(id, role);
        }

        /// <summary>
        /// Change User.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        /// <remarks>Changes user if user with "id" exists and new users properties are correct.</remarks>
        [Authorize]
        [HttpPut("{id}")]
        public ActionResult<string> Put(int id, [FromBody] User user)
        {
            return _usersService.ChangeUser(id, user);
        }

        /// <summary>
        /// Delete User by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>Delets user whit "id".</remarks>
        [Authorize]
        [HttpDelete("{id}")]
        public ActionResult<string> Delete(int id)
        {
            return _usersService.DeleteUser(id);
        }

        /// <summary>
        /// Get Authorization token.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <remarks>Returns token generated with users name.</remarks>
        [HttpGet("GetToken/{name}")]
        public ActionResult<object> GetToken(string name)
        {
            if (name is not null)
            {
                var claims = new List<Claim> { new Claim(ClaimTypes.Name, name) };
                var jwt = new JwtSecurityToken(
                        issuer: AuthorizationOpt.ISSUER,
                        audience: AuthorizationOpt.AUDIENCE,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(5)),
                        signingCredentials: new SigningCredentials(AuthorizationOpt.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
                return new JwtSecurityTokenHandler().WriteToken(jwt);
            }
            else return "Uncorrect name!";

        }
    }
}
