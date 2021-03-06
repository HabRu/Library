﻿using Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.ViewModels
{
    public class AllListBookViewModel
    {
        public IEnumerable<BookViewModel> Books { get; set; }

        public PageViewModel PageViewModel { get; set; }

        public FilterViewModel FilterViewModel { get; set; }

        public SortViewModel SortViewModel { get; set; }

        public List<Tracking> Trackings { get; set; }
    }
}
