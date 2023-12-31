
using CrudSystem.API.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Security.AccessControl;
using System.Text;

namespace CrudSystem.API.Data
{
    public class StorDbContext : DbContext
    {
        public StorDbContext(DbContextOptions<StorDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AuditTrail>().HasKey(A => A.Timestamp);
              

        }

        public DbSet<Products> products { get; set; }
        public DbSet<AuditTrail> auditTrails { get; set; }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var modifiedEntities = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added
                || e.State == EntityState.Modified
                || e.State == EntityState.Deleted)
                .ToList();
            foreach (var modifiedEntity in modifiedEntities)
            {

                var auditLog = new AuditTrail
                {
                    UserName = modifiedEntity.ToString(),
                    UserEmail = modifiedEntity.ToString(),
                    EntityName = modifiedEntity.Entity.GetType().Name,
                    Action = modifiedEntity.State.ToString(),
                    Timestamp = DateTime.UtcNow,
                    Changes = GetChanges(modifiedEntity)
                };
                auditTrails.Add(auditLog);
            }
            return base.SaveChangesAsync(cancellationToken);
        }
        private static string GetChanges(EntityEntry entity)
        {
            var changes = new StringBuilder();
            foreach (var property in entity.OriginalValues.Properties)
            {
                var originalValue = entity.OriginalValues[property];
                var currentValue = entity.CurrentValues[property];
                if (!Equals(originalValue, currentValue))
                {
                    changes.AppendLine($"{property.Name}: From '{originalValue}' to '{currentValue}'");
                }
            }
            return changes.ToString();
        }
    }
       
}

  
