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
            NameSort = sortOrder == SortState.NameAsc ? SortState.NameDesc : SortState.NameAsc;
            AuthorSort = sortOrder == SortState.AuthorAsc ? SortState.AuthorDesc : SortState.AuthorAsc;
            LangSort = sortOrder == SortState.LangAsc ? SortState.LangDesc : SortState.LangAsc;
            PubSort = sortOrder == SortState.PubAsc ? SortState.PubDesc : SortState.PubAsc;
            Current = sortOrder;




        }
    }
}
