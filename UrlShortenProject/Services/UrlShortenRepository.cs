using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UrlShortenProject.Models;

namespace UrlShortenProject.Services
{
    public class UrlShortenRepository:IUrlShortenRepository
    {
        private UrlShortenDbContext _urlContext;
        private string BASE_URL = @"https://far.de/";

        public UrlShortenRepository(UrlShortenDbContext urlContext)
        {
            _urlContext = urlContext;
        }
        public ICollection<UrlShorten> GetAllUrlShortens()
        {
            var urlShortens = _urlContext.UrlShortens.ToList();
            return urlShortens;
        }

        public bool IsValidUrl(string url)
        {
            Uri resultURI;
            if (!Regex.IsMatch(url, @"^https?:\/\/", RegexOptions.IgnoreCase))
                url = "http://" + url;

            if (Uri.TryCreate(url, UriKind.Absolute, out resultURI))
                return (resultURI.Scheme == Uri.UriSchemeHttp ||
                        resultURI.Scheme == Uri.UriSchemeHttps);
            return false;
        }

        public string AddHTTPPrefix(string url)
        {
            if (!url.StartsWith("http"))
            {
                url = "http://" + url;
            }

            return url;
        }

        public bool IsThisUrlAlreadyExists(string url)
        {
            var isExists = _urlContext.UrlShortens.Any(u => u.LongUrl == url);
            return isExists;
        }

        public bool IsShortenedUrlAlreadyExists(string url)
        {
            var isExists = _urlContext.UrlShortens.Any(u => u.ShortenedUrl == url);
            return isExists;
        }

        public string GenerateUniqueToken(string url)
        {
            string token = null;
            do
            {
                string urlsafe = "";
                Enumerable.Range(48, 75)
                    .Where(i => i < 58 || i > 64 && i < 91 || i > 96)
                    .OrderBy(o => new Random().Next())
                    .ToList()
                    .ForEach(i => urlsafe += Convert.ToChar(i)); // Store each char into urlsafe
                token = urlsafe.Substring(new Random().Next(0, urlsafe.Length), new Random().Next(2, 6));
            } while (_urlContext.UrlShortens.Any(u => u.Token == token));


            return token;
        }

        public UrlShorten GenerateUrlShortenObject(string url, string token)
        {
            var newObject = new UrlShorten()
            {
                LongUrl = url,
                Token = token,
                ShortenedUrl = BASE_URL + token
            };
            return newObject;
        }

        public bool CreateUrlShorten(UrlShorten objectToBeCreated)
        {
            _urlContext.Add(objectToBeCreated);
            return Save();
        }

        public bool Save()
        {
            var saved = _urlContext.SaveChanges();
            return saved >= 0 ? true : false;
        }

        public string GetLongerUrlOftheShortenedUrl(string url)
        {
            var longUrl=_urlContext.UrlShortens.Where(u=>u.ShortenedUrl==url).Select(u=>u.LongUrl).FirstOrDefault();
            return longUrl;

        }

        public string GetLongerUrlOfToken(string token)
        {
            var longUrl = _urlContext.UrlShortens.Where(u => u.Token == token).Select(u => u.LongUrl).FirstOrDefault();
            return longUrl;
        }
    }
}
