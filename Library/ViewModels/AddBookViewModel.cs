using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Library.ViewModels
{
    public class AddBookViewModel
    {

        [Required]
        [Display(Name ="Номер книжки")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Название")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Автор")]
        public string Authtor { get; set; }
        
        [Required]
        [Display(Name = "Год")]
        public int Year { get; set; }
        
        [Required]
        [Display(Name = "Язык")]
        public string Language { get; set; }
        
        [Required]
        [Display(Name = "Жанр")]
        public string Genre { get; set; }
        
        [Required]
        [Display(Name = "Издатель")]
        public string Publisher { get; set; }
        
        public IFormFile Image { get; set; }

    }
}
