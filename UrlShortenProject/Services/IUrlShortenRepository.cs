using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UrlShortenProject.Models;

namespace UrlShortenProject.Services
{
    public interface IUrlShortenRepository
    {
        ICollection<UrlShorten> GetAllUrlShortens();
        bool IsValidUrl(string url);
        string AddHTTPPrefix(string url);
        bool IsThisUrlAlreadyExists(string url);
        bool IsShortenedUrlAlreadyExists(string url);
        string GenerateUniqueToken(string url);
        UrlShorten GenerateUrlShortenObject(string url, string token);
        bool CreateUrlShorten(UrlShorten urlShorten);
        bool Save();
        string GetLongerUrlOftheShortenedUrl(string url);
        string GetLongerUrlOfToken(string token);
    }
}
