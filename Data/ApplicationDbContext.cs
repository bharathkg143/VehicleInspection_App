using Microsoft.EntityFrameworkCore;
using VIR_WebApp.Models;

namespace VIR_WebApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }

        public DbSet<VIRDocumentsUploads> VirDocumentsUploads { get; set; }
        public DbSet<GeneratedOTPs> GeneratedOTPs { get; set; }
        public DbSet<SMSTemplates> SMSTemplates { get; set; }
        public DbSet<SMSInteraction> SMSInteractions { get; set; }
        public DbSet<CustomerInfo> CustomerInfos { get; set; }
    }
}
