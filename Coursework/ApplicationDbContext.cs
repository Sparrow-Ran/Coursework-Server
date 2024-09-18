using Coursework.Models;
using Microsoft.EntityFrameworkCore;

namespace Coursework
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Issue> Issues { get; set; }
        public DbSet<IssueStatus> IssueStatuses { get; set; }
        public DbSet<IssuePriority> IssuePriorities { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<ProjectMember> ProjectMembers { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настройка отношений и ограничений

            // User - Role
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId);

            // Project - Manager (User)
            modelBuilder.Entity<Project>()
                .HasOne(p => p.Manager)
                .WithMany()
                .HasForeignKey(p => p.ManagerId);

            // Project - Issues
            modelBuilder.Entity<Issue>()
                .HasOne(i => i.Project)
                .WithMany(p => p.Issues)
                .HasForeignKey(i => i.ProjectId);

            // Issue - Status
            modelBuilder.Entity<Issue>()
                .HasOne(i => i.Status)
                .WithMany(s => s.Issues)
                .HasForeignKey(i => i.StatusId);

            // Issue - Priority
            modelBuilder.Entity<Issue>()
                .HasOne(i => i.Priority)
                .WithMany(p => p.Issues)
                .HasForeignKey(i => i.PriorityId);

            // Issue - Assignee (User)
            modelBuilder.Entity<Issue>()
                .HasOne(i => i.Assignee)
                .WithMany()
                .HasForeignKey(i => i.AssignedTo);

            // Issue - Comments
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Issue)
                .WithMany(i => i.Comments)
                .HasForeignKey(c => c.IssueId);

            // Comment - Author (User)
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Author)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.AuthorId);

            // Issue - Attachments
            modelBuilder.Entity<Attachment>()
                .HasOne(a => a.Issue)
                .WithMany(i => i.Attachments)
                .HasForeignKey(a => a.IssueId);

            // Attachment - Uploader (User)
            modelBuilder.Entity<Attachment>()
                .HasOne(a => a.Uploader)
                .WithMany(u => u.Attachments)
                .HasForeignKey(a => a.UploadedBy);

            // ProjectMember - Project
            modelBuilder.Entity<ProjectMember>()
                .HasOne(pm => pm.Project)
                .WithMany(p => p.ProjectMembers)
                .HasForeignKey(pm => pm.ProjectId);

            // ProjectMember - User
            modelBuilder.Entity<ProjectMember>()
                .HasOne(pm => pm.User)
                .WithMany(u => u.ProjectMembers)
                .HasForeignKey(pm => pm.UserId);

            // ProjectMember - Role
            modelBuilder.Entity<ProjectMember>()
                .HasOne(pm => pm.Role)
                .WithMany(r => r.ProjectMembers)
                .HasForeignKey(pm => pm.RoleId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
