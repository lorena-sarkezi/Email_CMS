using CMS.Data.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CMS.Data
{
    public class CmsDbContext : DbContext
    {
        public CmsDbContext(DbContextOptions<CmsDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }

        public virtual DbSet<Email> Emails { get; set; }
        public virtual DbSet<ConvoThread> Threads { get; set; }
        public virtual DbSet<Sender> Senders { get; set; }
        public virtual DbSet<Recepient> Recepients { get; set; }
    }
}
