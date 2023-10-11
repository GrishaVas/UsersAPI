namespace UsersAPI.Models.Business
{
    public class Role
    {
        public int Id { get; private set; }

        public string Name { get; set; }

        static public readonly List<Role> Roles = new List<Role>() {
            new Role("User"),
            new Role("Admin"),
            new Role("Support"),
            new Role("SuperAdmin")
        };

        public Role(string name)
        {
            Name = name;
        }

        public bool IsCorrect()
        {
            return Roles.Any(x => x.Name == Name);
        }
    }
}
