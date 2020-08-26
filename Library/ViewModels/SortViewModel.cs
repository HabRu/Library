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
            NameSort = sortOrder == SortState.NameAsc ? SortState.NameDesc : SortState.NameDesc;
            AuthorSort = sortOrder == SortState.AuthorAsc ? SortState.AuthorDesc : SortState.AuthorDesc;
            LangSort = sortOrder == SortState.LangAsc ? SortState.LangDesc : SortState.LangAsc;
            PubSort = sortOrder == SortState.PubAsc ? SortState.PubDesc : SortState.PubAsc;
            Current = sortOrder;
            
            
            
            //// значения по умолчанию
            //NameSort = SortState.NameAsc;
            //AuthorSort = SortState.AuthorAsc;
            //LangSort = SortState.LangAsc;
            //PubSort = SortState.PubAsc;
            //Up = true;

            //if (sortOrder == SortState.AuthorDesc || sortOrder == SortState.NameDesc
            //    || sortOrder == SortState.LangDesc||sortOrder==SortState.PubDesc)
            //{
            //    Up = false;
            //}

            //switch (sortOrder)
            //{
            //    case SortState.NameDesc:
            //        Current = NameSort = SortState.NameAsc;
            //        break;
            //    case SortState.AuthorAsc:
            //        Current = AuthorSort = SortState.AuthorDesc;
            //        break;
            //    case SortState.AuthorDesc:
            //        Current = AuthorSort = SortState.AuthorAsc;
            //        break;
            //    case SortState.LangAsc:
            //        Current = LangSort = SortState.LangDesc;
            //        break;
            //    case SortState.LangDesc:
            //        Current = LangSort = SortState.LangAsc;
            //        break;
            //    case SortState.PubDesc:
            //        Current = PubSort = SortState.PubAsc;
            //        break;
            //    case SortState.PubAsc:
            //        Current = PubSort = SortState.PubDesc;
            //        break;
            //    default:
            //        Current = NameSort = SortState.NameDesc;
            //        break;
            //}
        }
    }
}
