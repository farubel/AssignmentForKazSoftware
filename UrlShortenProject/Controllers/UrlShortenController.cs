using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UrlShortenProject.Models;
using UrlShortenProject.Services;

namespace UrlShortenProject.Controllers
{
    public class CustomModel
    {
        public string Url { get; set; }
    }
    [Route("/api")]
    [ApiController]
    public class UrlShortenController : Controller
    {
        private IUrlShortenRepository _urlShortenRepository;

        public UrlShortenController(IUrlShortenRepository urlShortenRepository)
        {
            _urlShortenRepository = urlShortenRepository;
        }


        [HttpGet()]
        [ProducesResponseType(400)]
        [ProducesResponseType(200,Type = typeof(IEnumerable<UrlShorten>))]
        public IActionResult GetAllUrls()
        {
            var urlShortens = _urlShortenRepository.GetAllUrlShortens();
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(urlShortens);
        }

        [HttpGet("short/{encodedUrl}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ProducesResponseType(200,Type = typeof(String))]
        [ProducesResponseType(404)]
        public IActionResult RedirectToOriginalUrl(string encodedUrl)
        {
            var shortUrl = System.Net.WebUtility.UrlDecode(encodedUrl);
            if (shortUrl == null)
            {
                return BadRequest(ModelState);
            }

            shortUrl = _urlShortenRepository.AddHTTPPrefix(shortUrl);
            if (!_urlShortenRepository.IsValidUrl(shortUrl))
            {
                ModelState.AddModelError(",", $"Invalid Url");
                StatusCode(422, ModelState);
            }

            var LongUrl = _urlShortenRepository.GetLongerUrlOftheShortenedUrl(shortUrl);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (LongUrl == null)
            {
                return NotFound();
            }

            return Ok(LongUrl);
        }

        [HttpGet("token/{token}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(String))]
        [ProducesResponseType(404)]
        public IActionResult RedirectToOriginalUrlFromToken(string token)
        {
            
            if (token == null)
            {
                return BadRequest(ModelState);
            }

            


            var LongUrl = _urlShortenRepository.GetLongerUrlOfToken(token);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (LongUrl == null)
            {
                return NotFound();
            }

            return Ok(LongUrl);
        }





        [HttpPost()]
        [ProducesResponseType(422)] //unprocessable entity
        [ProducesResponseType(405)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult PostUrl([FromBody] CustomModel customModel)
        {

            string urlName = customModel.Url;
            if (urlName == null)
            {
                return BadRequest(ModelState);
            }
           
            urlName = _urlShortenRepository.AddHTTPPrefix(urlName);
            if (!_urlShortenRepository.IsValidUrl(urlName))
            {
                ModelState.AddModelError(",",$"Invalid Url");
                StatusCode(422, ModelState);
            }
            

            if (_urlShortenRepository.IsThisUrlAlreadyExists(urlName))
            {
                ModelState.AddModelError("",$"{urlName} already exists");
                StatusCode(422, ModelState);
            }

            if (_urlShortenRepository.IsShortenedUrlAlreadyExists(urlName))
            {
                ModelState.AddModelError("",$"This url {urlName} is already shortened");
                StatusCode(405, ModelState);
            }

            var uniqueToken = _urlShortenRepository.GenerateUniqueToken(urlName);
            if (uniqueToken == null)
            {
                ModelState.AddModelError("","Can not generate Unique Token");
                StatusCode(422, ModelState);
            }

            var objectTobeCreated =  _urlShortenRepository.GenerateUrlShortenObject(urlName, uniqueToken);
           if (objectTobeCreated == null)
           {
               ModelState.AddModelError("", "Something went wrong");
               StatusCode(422, ModelState);

           }

           if (!ModelState.IsValid)
           {
               return BadRequest(ModelState);
           }
            if (!_urlShortenRepository.CreateUrlShorten(objectTobeCreated))
            { 
               ModelState.AddModelError("",$"Can not generate shorten url for {urlName}");
               StatusCode(500, ModelState);
            }
           return Ok(objectTobeCreated);

        }  
    }
}