using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models
{
    public static class ModelBulderConfigure
    {
        public static void InitializeData(this ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Book>(b =>
            {
                b.HasData(
                new Book[]
                {
                    new Book("") {
                        Id=5,
                        Title = "Мир и война",
                        Authtor = "Борис Акунин",
                        Language = "Русский",
                        Genre = "Детектив",
                        Publisher="Литрес",
                        Description = "Детективный роман Бориса Акунина, действие которого разворачивается на фоне грозных событий войны 1812 года, является художественным приложением к седьмому тому проекта «История Российского государства». Такой пары сыщиков в истории криминального жанра, кажется, еще не было",
                        Image = "/images/миривойна.jpg",
                    }
                });

            });

            modelBuilder.Entity<Evaluation>(e =>
            {
                e.HasData(new Evaluation[]
                {
                    new Evaluation()
                    {
                        Id = 5,
                        Average = 0,
                        BookId = 5,

                    }
                });
            });

        }
    }
}
