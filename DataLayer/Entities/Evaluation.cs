﻿using System.Collections.Generic;

namespace Library.Models
{
    public class Evaluation
    {
        public int Id { get; set; }

        public int BookId { get; set; }

        public Book Book { get; set; }

        public byte Average { get; set; }

        public List<string> Users { get; set; }


    }

}