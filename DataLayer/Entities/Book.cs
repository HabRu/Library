﻿using DataLayer.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models
{
    public class Book : IEntity<int>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Title { get; set; }

        public string Authtor { get; set; }

        public int Year { get; set; }

        public string Language { get; set; }

        public string Genre { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public Evaluation Evaluation { get; set; }

        public ICollection<Reservation> Reservations { get; set; }

        public Status Status { get; set; }

        public string Publisher { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

        public List<Tracking> TrackingList { get; set; }

        public Book(string name)
        {
            Description = "Нет описания";
            Comments = new List<Comment>();
            TrackingList = new List<Tracking>();
            Reservations = new List<Reservation>();
            Image = "/images/NoImage.jpg";
        }

        public Book()
        {
            Description = "Нет описания";
            Comments = new List<Comment>();
            Reservations = new List<Reservation>();
            Evaluation = new Evaluation()
            {
                Id = Id,
                Average = 0,
                BookId = Id,
                Users = new List<string>()

            };
            TrackingList = new List<Tracking>();
            Image = "/images/NoImage.jpg";
            Status = Status.Available;
        }
    }
}
