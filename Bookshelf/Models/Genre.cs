using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bookshelf.Models
{
    public class Genre
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public List<BookGenre> BookGenres { get; set; }
    }
}
