using Microsoft.EntityFrameworkCore;

namespace PersonasNaturalesG9.Models
{
    public class PersonasDbContext : DbContext
    {
        public PersonasDbContext(DbContextOptions<PersonasDbContext> options) : base(options) { }
        public DbSet<Persona> Persona { get; set; }

    }
}
