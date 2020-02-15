using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UrlShortenProject.Models;

namespace UrlShortenProject
{
    public static class DbSeedingClass
    {
        public static void SeedDataContext(this UrlShortenDbContext context)
        {
            var urlShortens = new List<UrlShorten>
            {
                new UrlShorten()
                {
                    LongUrl = @"https://www.google.com/",
                    ShortenedUrl = @"https://far.de/mnopq",
                    Token = "mnopq",
                },
                new UrlShorten()
                {
                    LongUrl = @"https://www.espncricinfo.com/story/_/id/28686811/bangladesh-19-champions-receive-heroes-welcome-mirpur",
                    ShortenedUrl = @"https://far.de/UVW",
                    Token = @"UVW"
                },
                new UrlShorten()
                {
                    LongUrl = @"https://en.wikipedia.org/wiki/Birthday_problem",
                    ShortenedUrl = @"https://far.de/HIJ",
                    Token = "HIJ"
                }
            };
              context.UrlShortens.AddRange(urlShortens);
              context.SaveChanges();
        }
    }
}

