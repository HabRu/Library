using Microsoft.EntityFrameworkCore;

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
                        Id=1,
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

            modelBuilder.Entity<Book>(b =>
            {
                b.HasData(
                new Book[]
                {
                    new Book("") {
                        Id=4,
                        Title = "Мир и дружба",
                        Authtor = "Иван Петров",
                        Language = "Русский",
                        Genre = "Комедия",
                        Publisher="Старт",
                        Description = "Эывыфвыфвыфвыфваыавы",
                        Image = "/images/NoImage.jpg",
                    }
                });
            });

            modelBuilder.Entity<Book>(b =>
            {
                b.HasData(
                new Book[]
                {
                    new Book("") {
                        Id=5,
                        Title = "Безмолвный пациент",
                        Authtor = "Алекс Михаэлидес",
                        Language = "Английский",
                        Genre = "Триллер",
                        Publisher="Эксмо",
                        Description = "Когда художнице Алисии было тридцать три года, она убила своего мужа.С тех пор прошло шесть лет. За это время она не произнесла ни слова.",
                        Image = "/images/безмолвпациент.jpg",
                    }
                });
            });

            modelBuilder.Entity<Book>(b =>
            {
                b.HasData(
                new Book[]
                {
                    new Book("") {
                        Id=2,
                        Title = "Цена вопросв",
                        Authtor = "Александра Маринина",
                        Language = "Русский",
                        Genre = "Детектив",
                        Publisher="Литрес",
                        Description = "Программа против Cистемы. Системы всесильной и насквозь коррумпированной, на все имеющей цену и при этом ничего неспособной ценить по-настоящему. Возможно ли такое?",
                        Image = "/images/ценавопроса.jpg",
                    }
                });
            });

            modelBuilder.Entity<Book>(b =>
            {
                b.HasData(
                new Book[]
                {
                    new Book("") {
                        Id=3,
                        Title = "Внутри убийцы",
                        Authtor = "Майк Омер",
                        Language = "Русский",
                        Genre = "Детектив",
                        Publisher="Литрес",
                        Description = "На мосту в Чикаго, облокотившись на перила, стоит молодая красивая женщина. Очень бледная и очень грустная. Она неподвижно смотрит на темную воду, прикрывая ладонью плачущие глаза. И никому не приходит в голову, что",
                        Image = "/images/внутриубийцы.jpg",
                    }
                });
            });

            modelBuilder.Entity<Evaluation>(e =>
            {
                e.HasData(new Evaluation[]
                {
                    new Evaluation()
                    {
                        Id = 1,
                        Average = 0,
                        BookId = 1,
                    }
                }); ; ;
            });

            modelBuilder.Entity<Evaluation>(e =>
            {
                e.HasData(new Evaluation[]
                {
                    new Evaluation()
                    {
                        Id = 2,
                        Average = 0,
                        BookId = 2,
                    }
                });
            });
            modelBuilder.Entity<Evaluation>(e =>
            {
                e.HasData(new Evaluation[]
                {
                    new Evaluation()
                    {
                        Id = 3,
                        Average = 0,
                        BookId = 3,
                    }
                });
            });

            modelBuilder.Entity<Evaluation>(e =>
            {
                e.HasData(new Evaluation[]
                {
                    new Evaluation()
                    {
                        Id = 4,
                        Average = 0,
                        BookId = 4,
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
