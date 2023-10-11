using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace UsersAPI.Models
{
    public class AuthorizationOpt
    {
        public const string ISSUER = "UsersApiServer"; // издатель токена
        public const string AUDIENCE = "UsersApiClient"; // потребитель токена
        const string KEY = "123sdkfmsdfk1232233fkdsfks";   // ключ для шифрации
        public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
    }
}
