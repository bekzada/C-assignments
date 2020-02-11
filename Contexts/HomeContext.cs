using Microsoft.EntityFrameworkCore;
using DemoLoginRegistration.Models;

namespace DemoLoginRegistration.Contexts
{
    public class HomeContext : DbContext 
    {
        public HomeContext(DbContextOptions options) : base(options){} 

        public DbSet<User> Users{ get; set;} //pluralise and Upper case
    }
}