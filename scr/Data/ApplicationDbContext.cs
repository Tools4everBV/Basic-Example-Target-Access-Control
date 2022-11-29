using EXAMPLE.API.Access.Control.Data.Models;
using Microsoft.EntityFrameworkCore;


namespace EXAMPLE.API.Access.Control.Data
{
    /// <summary>
    /// Basic dbcontext class to make this API actually do something.
    /// Note that migrations are not yet created!
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User>? User { get; set; }
        public DbSet<RoleAuthorization>? Authorization { get; set; }
        public DbSet<Role>? Role { get; set; }
        public DbSet<AccessKey>? AccessKey { get; set; }
        public DbSet<AccessKeyAssignments>? AccessKeyAssignment { get; set; }

        /// <summary>
        /// This method configures EFCore so that EFCore knows where to look for the database.
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var conn = $@"Data Source={baseDir}//EXAMPLE.db";
            optionsBuilder.UseSqlite(conn);
        }
    }
}
