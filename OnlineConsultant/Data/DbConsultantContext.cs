using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using OnlineConsultant.Data.Entitys;

namespace OnlineConsultant.Data
{
    public class DbConsultantContext : DbContext
    {
        public DbSet<Question> Questions { get; set; }

        public DbSet<User> User { get; set; }

        public DbConsultantContext()
            : base("DbConnection")
        {
            Database.SetInitializer(new System.Data.Entity.MigrateDatabaseToLatestVersion<DbConsultantContext, Configuration> ());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var tableUser = modelBuilder.Entity<User>();
            var tableQuestion = modelBuilder.Entity<Question>();

            //Table Users
            tableUser.HasKey(x => x.Id)
                     .Property(x => x.Id)
                     .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            tableUser.Property(x => x.Surname).HasMaxLength(255);
            tableUser.Property(x => x.Name).HasMaxLength(255);
            tableUser.Property(x => x.Firstname).HasMaxLength(255);
                    
            //Table Question
            tableQuestion.HasKey(x => x.Id)
                         .Property(x => x.Id)
                         .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            tableQuestion.Property(x => x.CreateTime).IsOptional();
            tableQuestion.Property(x => x.Description);

            // Configure User & Question entity
            tableQuestion.HasRequired(x => x.User)
                         .WithMany(s => s.Questions);
        }
    }
}