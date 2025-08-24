using Dating_App.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dating_App.Data
{
    /// <summary>
    /// Represents the application's database context, providing access to the database and enabling CRUD operations for
    /// the application's entities.
    /// </summary>
    /// <remarks>This class is derived from <see cref="Microsoft.EntityFrameworkCore.DbContext"/> and is
    /// configured with the specified <see cref="Microsoft.EntityFrameworkCore.DbContextOptions"/>. It serves as the
    /// primary entry point for interacting with the database using Entity Framework Core.</remarks>
    /// <param name="options"></param>
    public class AppDbContext(DbContextOptions options) : DbContext(options)
    {
        /// <summary>
        /// Represents the collection of <see cref="AppUser"/> entities in the database.
        /// </summary>
        /// <remarks>This property provides access to the <see cref="DbSet{TEntity}"/> for performing CRUD
        /// operations on <see cref="AppUser"/> entities. Use this property to query, add, update, or delete users in
        /// the database.</remarks>
        public virtual DbSet<AppUser> Users { get; set; }
    }
}
