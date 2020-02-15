using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace UrlShortenProject.Models
{
    public class UrlShorten
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(200)]
        public string LongUrl { get; set; }
        [Required]
        [MaxLength(30)]
        public string ShortenedUrl { get; set; }
        [Required]
        [MaxLength(10)]
        public string Token { get; set; }

        public DateTime? Created { get; set; } = DateTime.Now;

    }
}
