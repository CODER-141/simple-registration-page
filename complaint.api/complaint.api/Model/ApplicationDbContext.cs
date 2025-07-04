using Microsoft.EntityFrameworkCore;
using System;

namespace complaint.api.Model {
    public class ApplicationDbContext : DbContext {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
          : base(options) {
        }

        public DbSet<UserModel> Users { get; set; }
    }
}
