
namespace Library.Models
{
    public  class MessageForm
    {
        public string GetMessage(string email,string bookTitle)
        {
            string message = $"<b>Дорогой {email}</b>, книгу <b>\"{bookTitle}\"</b> снова можно забронировать!!!";
            return message;
        }
    }
}
