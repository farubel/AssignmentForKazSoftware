using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UrlShortenProject.Models;

namespace UrlShortenProject
{
    public class UrlShortenDbContext: DbContext
    {
        public UrlShortenDbContext(DbContextOptions<UrlShortenDbContext> options):base(options)
        {
            Database.Migrate();
        }

        public  DbSet<UrlShorten> UrlShortens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

    }
}
