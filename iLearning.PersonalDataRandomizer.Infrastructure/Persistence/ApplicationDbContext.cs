using iLearning.PersonalDataRandomizer.Domain.Models.Data.City;
using iLearning.PersonalDataRandomizer.Domain.Models.Data.Name;
using iLearning.PersonalDataRandomizer.Domain.Models.Data.Patronymic;
using iLearning.PersonalDataRandomizer.Domain.Models.Data.Street;
using iLearning.PersonalDataRandomizer.Domain.Models.Data.Surname;
using Microsoft.EntityFrameworkCore;

namespace iLearning.PersonalDataRandomizer.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
		: base(options)
	{ }

    public DbSet<RuName> RuNames { get; set; }
    public DbSet<RuSurname> RuSurnames { get; set; }
    public DbSet<RuPatronymic> RuPatronymics { get; set; }
    public DbSet<RuCity> RuCities { get; set; }
    public DbSet<RuStreet> RuStreets { get; set; }

    public DbSet<PlName> PlNames { get; set; }
    public DbSet<PlSurname> PlSurnames { get; set; }
    public DbSet<PlCity> PlCities { get; set; }
    public DbSet<PlStreet> PlStreets { get; set; }

    public DbSet<UsName> UsNames { get; set; }
    public DbSet<UsSurname> UsSurnames { get; set; }
    public DbSet<UsCity> UsCities { get; set; }
    public DbSet<UsStreet> UsStreets { get; set; }
}
