using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PunchList.Models;

namespace PunchList.Data;
public class ApplicationDbContext : IdentityDbContext<ApplicationUser> {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Project> Projects => Set<Project>();
    public DbSet<TaskItem> TaskItems => Set<TaskItem>();
    public DbSet<SubTaskItem> SubTaskItems => Set<SubTaskItem>();

    protected override void OnModelCreating(ModelBuilder b) {
        base.OnModelCreating(b);

        // Relations + cascade
        b.Entity<Project>()
            .HasMany(p => p.Tasks)
            .WithOne()
            .HasForeignKey(t => t.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        b.Entity<TaskItem>()
            .HasMany(t => t.SubTasks)
            .WithOne()
            .HasForeignKey(s => s.TaskItemId)
            .OnDelete(DeleteBehavior.Cascade);

        // Constraints
        b.Entity<TaskItem>()
            .HasIndex(t => new { t.ProjectId, t.Status, t.Order });

        b.Entity<SubTaskItem>()
            .HasIndex(s => new { s.TaskItemId, s.Order });
    }
}
