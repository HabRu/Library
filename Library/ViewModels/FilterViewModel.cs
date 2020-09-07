using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.ViewModels
{
    public class FilterViewModel
    {
        public string SelectedTitle;
        public string SelectedLanguage;
        public string SelectedAuthor;
        public string SelectedGenre;
        public string SelectedPublisher;
        public FilterViewModel(string title, string language, string author, string genre, string publisher)
        {

            SelectedTitle = title;
            SelectedLanguage = language;
            SelectedAuthor = author;
            SelectedGenre = genre;
            SelectedPublisher = publisher;
        }
    }
}
