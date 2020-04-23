using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bookshelf.Models.ViewModel
{
    public class BookFormViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Author { get; set; }
        public List<SelectListItem> GenreOptions { get; set; }
        public List<int> SelectGenreIds { get; set; }
    }
}
