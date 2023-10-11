using UsersAPI.Models.Business;

namespace UsersApiTests
{
    public class UsersServiceTests
    {
        [Theory]
        [InlineData(0, "name", 23, "email@asd.com")]
        [InlineData(-1, "name", 23, "email@asd.com")]
        [InlineData(1, "", 23, "email@asd.com")]
        [InlineData(1, "name", 0, "email@asd.com")]
        [InlineData(1, "name", -1, "email@asd.com")]
        [InlineData(1, "name", 23, "emailasd.com")]
        [InlineData(1, "name", 23, "@asd.com")]
        [InlineData(1, "name", 23, "emailasd@.com")]
        [InlineData(1, "name", 23, "emailasd@com")]
        [InlineData(1, "name", 23, "emailasd.@com")]
        [InlineData(1, "name", 23, "email@asd.")]
        [InlineData(1, "name", 23, "email@.com")]
        public void CheckUserUncorrect(long id, string name, int age, string email)
        {
            User user = new User(id, name, age, email);
            Assert.False(user.IsCorrect());
        }

        [Theory]
        [InlineData(1, "name", 23, "email@asd.com","User")]
        [InlineData(1, "name", 23, "email@asd.com", "Admin")]
        [InlineData(1, "name", 23, "email@asd.com", "SuperAdmin")]
        [InlineData(1, "name", 23, "email@asd.com", "Support")]
        public void CheckUserCorrect(long id, string name, int age, string email, string roleName)
        {
            List<Role> roles = new List<Role>() { new Role(roleName)};
            User user = new User(id, name, age, email, roles);
            Assert.True(user.IsCorrect());
        }

        [Theory]
        [InlineData("User")]
        [InlineData("Admin")]
        [InlineData("SuperAdmin")]
        [InlineData("Support")]
        public void CheckRoleCorrect(string name)
        {
            Role role = new Role(name);
            Assert.True(role.IsCorrect());
        }

        [Theory]
        [InlineData("Usr")]
        [InlineData("Admin_")]
        [InlineData("_SuperAdmin")]
        [InlineData("Support.")]
        [InlineData("")]
        public void CheckRoleUncorrect(string name)
        {
            Role role = new Role(name);
            Assert.False(role.IsCorrect());
        }
    }
}