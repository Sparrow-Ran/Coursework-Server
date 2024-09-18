using Coursework.Models;

namespace Coursework.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            if (!context.Roles.Any())
            {
                var roles = new List<Role>
            {
                new Role { Name = "Admin" },
                new Role { Name = "User" }
            };

                context.Roles.AddRange(roles);
                context.SaveChanges();
            }
        }
    }
}
