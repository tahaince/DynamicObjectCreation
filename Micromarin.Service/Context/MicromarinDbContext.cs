using Micromarin.Services.Entities;
using Micromarin.Services.Infastructures.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace Micromarin.Services.Context;

public class MicromarinDbContext : DbContext {
    public DbSet<DynamicEntity> DynamicEntities { get; set; }
    public MicromarinDbContext(DbContextOptions options) : base(options) { }

    //protected override void OnModelCreating(ModelBuilder modelBuilder) {
    //}
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        optionsBuilder.AddInterceptors(new SavingChangesInterceptor());
        base.OnConfiguring(optionsBuilder);
    }
}
