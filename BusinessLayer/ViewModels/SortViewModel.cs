using Library.Tag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.ViewModels
{
    public class SortViewModel
    {
        public SortState NameSort { get; set; }

        public SortState AuthorSort { get; set; }

        public SortState LangSort { get; set; }

        public SortState PubSort { get; set; }

        public SortState Current { get; set; }


        public SortViewModel(SortState sortOrder)
        {
            NameSort = sortOrder == SortState.TitleAsc ? SortState.TitleDsc : SortState.TitleAsc;
            AuthorSort = sortOrder == SortState.AuthtorAsc ? SortState.AuthtorDsc : SortState.AuthtorAsc;
            LangSort = sortOrder == SortState.LanguageAsc ? SortState.LanguageDsc : SortState.LanguageAsc;
            PubSort = sortOrder == SortState.PublisherAsc ? SortState.PublisherDsc : SortState.PublisherAsc;
            Current = sortOrder;
        }
    }
}
